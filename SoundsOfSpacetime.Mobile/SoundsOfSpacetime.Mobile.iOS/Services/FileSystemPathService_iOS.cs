using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SoundsOfSpacetime.Mobile.Interfaces;
using UIKit;

namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class FileSystemPathService_iOS : IFileSystemPathService
    {
        public string GetDownloadsPath()
        {
            var downloadsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
            return downloadsPath;
        }
    }
}