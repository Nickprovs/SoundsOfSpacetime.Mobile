using Expandable;
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

        private async void ExpandableView_StatusChanged(object sender, Expandable.StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case ExpandStatus.Expanding:
                    await this.TopHalfScrollViewer?.ScrollToAsync(this.AdditionalOptions, ScrollToPosition.Start, true);
                    break;
                default:
                    break;
            }
        }
    }
}
