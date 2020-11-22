﻿using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Prism;
using Prism.Ioc;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.Droid.Services;
using MediaManager;
using Xamarin.Essentials;
using SoundsOfSpacetime.Mobile.Services;

namespace SoundsOfSpacetime.Mobile.Droid
{
    //We'll set the main style to the splash here, and once we've created... we'll set it back to the MainTheme
    [Activity(Label = "SoundsOfSpacetime.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlatformInitializer
    {

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle

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

            //Init Cross Media Manager (For GW Sound Files)
            CrossMediaManager.Current.Init();

            //Init Oxyplot
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            //Once we've loaded in... let's set our theme back to the main theme.
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState, persistentState);
        }
    }
}