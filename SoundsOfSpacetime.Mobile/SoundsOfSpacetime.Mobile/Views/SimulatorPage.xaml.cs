using Expandable;
using SoundsOfSpacetime.Mobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.Views
{
    public partial class SimulatorPage : ContentPage
    {
        public SimulatorPage()
        {
            InitializeComponent();
            var nativePlotService = DependencyService.Resolve<IPlotService>();
            this.PlotContainer.Content = nativePlotService.Render();
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
