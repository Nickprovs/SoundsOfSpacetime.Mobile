using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nickprovs.Albatross.Interfaces
{
    public interface IPlotService
    {
        View Render();


        void Plot(IEnumerable<IPoint> dataSeries);

        void PlotAnimated(IEnumerable<IPoint> dataSeries, double desiredTimeInMillis);

        void Clear();

        void SetXAxisTitle(string xAxistTitle);

        void SetYAxisTitle(string yAxisTitle);

        void SetTitle(string title);
    }
}
