using System.Drawing;
using MyContacts.Models;

namespace MyContacts.Interfaces
{
    public interface IEnvironment
    {
        Theme GetOSTheme();
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
