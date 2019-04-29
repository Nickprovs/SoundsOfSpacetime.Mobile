using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nickprovs.Albatross.Views
{
    public partial class SimulatorPage : ContentPage
    {
        public SimulatorPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!ExpandableOptions.IsVisible)
            {
                ExpandableOptions.IsVisible = true;
                await Task.Delay(25);
                await this.TopHalfScrollViewer.ScrollToAsync(0, TopHalf.Height, false);
            }
            else
            {
                await this.TopHalfScrollViewer.ScrollToAsync(0, AdditionalOptions.Y, false);
                ExpandableOptions.IsVisible = false;
            }
        }
    }
}
