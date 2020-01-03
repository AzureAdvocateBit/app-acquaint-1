using System.Drawing;
using Acquaint.Models;

namespace Acquaint.Interfaces
{
    public interface IEnvironment
    {
        Theme GetOSTheme();
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
