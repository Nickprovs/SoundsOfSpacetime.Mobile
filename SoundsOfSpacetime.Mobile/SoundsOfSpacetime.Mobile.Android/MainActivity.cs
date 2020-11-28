﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.Droid.Services;
using MediaManager;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms;
using SoundsOfSpacetime.Mobile.Droid.Services.AudioDeviceMonitoring;
using Android.Content;

namespace SoundsOfSpacetime.Mobile.Droid
{
    //We'll set the main style to the splash here, and once we've created... we'll set it back to the MainTheme
    [Activity(Label = "SoundsOfSpacetime.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlatformInitializer
    {

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAudioDeviceMonitor, AudioDeviceMonitor_Android>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //Init Third Party Modules
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            CrossMediaManager.Current.Init();

            //Basic Init
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(this));

            //These permissions may not be necessary... And you should ask them at feature exec (and deny if not granted)... not here
            Device.InvokeOnMainThreadAsync(this.RequestPermissions);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartService(new Intent(this, typeof(HeadsetMonitoringService)));
        }

        private async Task RequestPermissions()
        {
            var readPermisson = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            var writePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (readPermisson != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            if (writePermission != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
        }
    }
}