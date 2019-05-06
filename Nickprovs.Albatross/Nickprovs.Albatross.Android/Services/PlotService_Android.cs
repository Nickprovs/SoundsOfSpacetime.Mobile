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
using Debug = System.Diagnostics.Debug;

[assembly: Dependency(typeof(PlotService_Android))]
namespace Nickprovs.Albatross.Droid.Services
{
    public class PlotService_Android : IPlotService
    {
        public void Render(ContentView plotContainer)
        {
            Debug.WriteLine("Called from Android Platform Service!");
        }


    }
}