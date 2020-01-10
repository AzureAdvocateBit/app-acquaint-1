using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MyContacts.Interfaces;
using MyContacts.Models;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;

[assembly: Dependency(typeof(MyContacts.UWP.Helpers.Environment))]
namespace MyContacts.UWP.Helpers
{
    public class Environment : IEnvironment
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
        }
    }
}