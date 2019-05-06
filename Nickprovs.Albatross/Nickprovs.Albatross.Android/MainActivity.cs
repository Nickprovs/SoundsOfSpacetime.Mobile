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

namespace Nickprovs.Albatross.Droid
{
    //We'll set the main style to the splash here, and once we've created... we'll set it back to the MainTheme
    [Activity(Label = "Nickprovs.Albatross", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlatformInitializer
    {

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPlotService, PlotService_Android>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            SciChart.Charting.Visuals.SciChartSurface.SetRuntimeLicenseKey(@"<LicenseContract>
              <Customer>provost.nikolai@gmail.com</Customer>
              <OrderId>Trial</OrderId>
              <LicenseCount>1</LicenseCount>
              <IsTrialLicense>true</IsTrialLicense>
              <SupportExpires>05/12/2019 00:00:00</SupportExpires>
              <ProductCode>SC-ANDROID-2D-ENTERPRISE-SRC</ProductCode>
              <KeyCode>58584212ac996f6d1b3f9137aedcd12c041194c58666a4b800f5673b028d61657aded3c45533780579c1b0fdb21b3b1a1e52ee5959451e22cb0190ccaa140dccddace7ee6769692b52523459ed6001302a7eac12dc9c16f46b82d88bea97b5b2498c5999b893882435911d5a9fd7ce278e526f1440bee07e35bb4dc208c0834b99e88da1fb42c3afb94dc0fe49f72d4abfd488fcf2b3b96fcb05d5de6f952e5ea2b76ca8d3a6b3a0fe19c3eeef8a7bd139df</KeyCode>
              </LicenseContract>");

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