using System;
using System.IO;
using Android.OS;
using Nickprovs.Albatross.Droid.Services;
using Nickprovs.Albatross.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystemPathService_Android))]
namespace Nickprovs.Albatross.Droid.Services
{
    public class FileSystemPathService_Android : IFileSystemPathService
    {
        public string GetDownloadsPath()
        {
            var downloadsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
            return downloadsPath;
        }
    }
}