using System.Threading;
using SoundsOfSpacetime.Mobile.Interfaces;
using Xamarin.Forms;
namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class ShutdownService_iOS : IShutdownService
    {
        public void Shutdown()
        {
            Xamarin.Forms.Application.Current.Quit();
        }
    }
}