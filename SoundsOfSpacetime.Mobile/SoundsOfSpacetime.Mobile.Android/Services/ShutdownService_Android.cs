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
using SoundsOfSpacetime.Mobile.Interfaces;

namespace SoundsOfSpacetime.Mobile.Droid.Services
{
    public class ShutdownService_Android : IShutdownService
    {
        public void Shutdown()
        {
            System.Environment.Exit(0);
        }
    }
}