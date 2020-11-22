using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(GenericFileSystemPathService))]
namespace SoundsOfSpacetime.Mobile.Services
{
    public class GenericFileSystemPathService : IFileSystemPathService
    {
        public string GetDownloadsPath()
        {
            var downloadsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
            return downloadsPath;
        }
    }
}
