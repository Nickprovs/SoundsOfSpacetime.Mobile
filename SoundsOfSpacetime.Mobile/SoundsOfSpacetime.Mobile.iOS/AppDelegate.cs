using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using MediaManager;
using Prism;
using Prism.Ioc;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.iOS.Services;
using SoundsOfSpacetime.Mobile.Services;

namespace SoundsOfSpacetime.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IPlatformInitializer
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            //Init Oxyplot
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();

            //Init Cross Media Manager (For GW Sound Files)
            CrossMediaManager.Current.Init();

            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
