using System;
using System.Collections.Generic;
using System.Linq;
using SciChart;
using Foundation;
using UIKit;
using MediaManager;

namespace Nickprovs.Albatross.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
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
            LoadApplication(new App());

            //Init SciChart Licensing
            SciChart.iOS.Charting.SCIChartSurface.SetRuntimeLicenseKey(@"<LicenseContract>
             <Customer>Montclair State University</Customer>
             <OrderId>EDUCATIONAL - USE - 0046</OrderId>
             <LicenseCount>1</LicenseCount>
             <IsTrialLicense>false</IsTrialLicense>
             <SupportExpires>12/13/2018 00:00:00</SupportExpires>
             <ProductCode>SC-IOS-ANDROID-2D-PRO</ProductCode>
             <KeyCode>d7b9c5debf3e0bd0e76443892ed0cb87f73f99654fd71d4a104106e4dd822130dfc0abdc365c644a2de4152a063ff540532b426126d9f32e7fed6c46ece72801b9e68c2539da3769954c83d927f8e333093fc321e6754bfdb7e48fbf8f2c1290264568b9787d96caa569f7e5c9975272f0fd5d5b76bb0219ae0b98146f94225a0c5aca1214aa19fab4be4736f368856c951bc5241969f29ecd3efd8d4dfb19d7dc832d54ede76a2c105c213c1288db42321425ea777c4c7228ac80d2</KeyCode>
             </LicenseContract>");

            //Init Cross Media Manager (For GW Sound Files)
            CrossMediaManager.Current.Init();

            //Init Oxyplot
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();


            return base.FinishedLaunching(app, options);
        }
    }
}
