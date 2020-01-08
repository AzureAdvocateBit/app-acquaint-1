using System.Drawing;
using MyContacts.Models;

namespace MyContacts.Interfaces
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
