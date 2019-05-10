using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
            var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            var absolutePath = downloadsPath.AbsolutePath;
            return absolutePath;
        }
    }
}