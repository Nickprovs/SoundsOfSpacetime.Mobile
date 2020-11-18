using System;
using System.IO;
using Android.OS;
using SoundsOfSpacetime.Mobile.Droid.Services;
using SoundsOfSpacetime.Mobile.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystemPathService_Android))]
namespace SoundsOfSpacetime.Mobile.Droid.Services
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