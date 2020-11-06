using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism;
using Prism.Ioc;
using Nickprovs.Albatross.Interfaces;
using Nickprovs.Albatross.Droid.Services;
using MediaManager;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;

namespace Nickprovs.Albatross.Droid
{
    //We'll set the main style to the splash here, and once we've created... we'll set it back to the MainTheme
    [Activity(Label = "Nickprovs.Albatross", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlatformInitializer
    {

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPlotService, PlotService_Android>();
            containerRegistry.RegisterSingleton<IFileSystemPathService, FileSystemPathService_Android>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //Init SciChart Licensing
            SciChart.Charting.Visuals.SciChartSurface.SetRuntimeLicenseKey(@"<LicenseContract>
             <Customer>Montclair State University</Customer>
             <OrderId>EDUCATIONAL - USE - 0046</OrderId>
             <LicenseCount>1</LicenseCount>
             <IsTrialLicense>false</IsTrialLicense>
             <SupportExpires>12/13/2018 00:00:00</SupportExpires>
             <ProductCode>SC-IOS-ANDROID-2D-PRO</ProductCode>
             <KeyCode>d7b9c5debf3e0bd0e76443892ed0cb87f73f99654fd71d4a104106e4dd822130dfc0abdc365c644a2de4152a063ff540532b426126d9f32e7fed6c46ece72801b9e68c2539da3769954c83d927f8e333093fc321e6754bfdb7e48fbf8f2c1290264568b9787d96caa569f7e5c9975272f0fd5d5b76bb0219ae0b98146f94225a0c5aca1214aa19fab4be4736f368856c951bc5241969f29ecd3efd8d4dfb19d7dc832d54ede76a2c105c213c1288db42321425ea777c4c7228ac80d2</KeyCode>
             </LicenseContract>");

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 0);
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