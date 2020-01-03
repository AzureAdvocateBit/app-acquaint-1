using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Acquaint.Util;
using Acquaint.iOS.Renderers;
using Acquaint.Models;
using Acquaint.Styles;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(DefaultPageRenderer))]
namespace Acquaint.iOS.Renderers
{

    public class DefaultPageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);


            if (Settings.ThemeOption != Theme.Default)
                return;

            Console.WriteLine($"TraitCollectionDidChange: {TraitCollection.UserInterfaceStyle} != {previousTraitCollection.UserInterfaceStyle}");

            if (TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
                SetAppTheme();
        }

        void SetAppTheme()
        {
            if (TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
                ThemeHelper.ChangeTheme(Theme.Dark);
            else
                ThemeHelper.ChangeTheme(Theme.Light);
        }
    }
}