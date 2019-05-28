using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Nickprovs.Albatross.Droid.Services;
using Nickprovs.Albatross.Droid.Utilities;
using Nickprovs.Albatross.Interfaces;
using SciChart.Charting.Model;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Modifiers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.Common;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Debug = System.Diagnostics.Debug;

[assembly: Dependency(typeof(PlotService_Android))]
namespace Nickprovs.Albatross.Droid.Services
{
    public class PlotService_Android : IPlotService
    {
        #region Fields

        /// <summary>
        /// The plotting surface
        /// </summary>
        private SciChartSurface _plottingSurface;

        /// <summary>
        /// The data driving series drawn on the plotting surface
        /// </summary>
        private XyDataSeries<double,double> _series;

        /// <summary>
        ///The view drawing the data series   
        /// </summary>
        private FastLineRenderableSeries _renderableSeries;

        /// <summary>
        /// The x-axis
        /// </summary>
        NumericAxis _xAxis;

        /// <summary>
        /// The y-axis
        /// </summary>
        NumericAxis _yAxis;

        /// <summary>
        /// The animation timer
        /// </summary>
        private Timer _animationTimer;

        /// <summary>
        /// The animation delay time when performing animations
        /// </summary>
        private readonly double _animationDelayTime;

        #endregion

        #region Contructors and Destructors

        /// <summary>
        /// Sets the necessary lifetime data
        /// </summary>
        public PlotService_Android()
        {
            //This timer will handle all of the waiting between point appendages to give the appearance of animation.
            this._animationTimer = new Timer();

            //SciChart Advises appending points in batches of 10-100 for performance. We'll go with the upper bound.
            this._animationDelayTime = 100;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Takes in a Xamarin Forms ContentView and adds the plot surface as a child to that view.
        /// TODO: Rename this function to something more appropriate and have it just return the ToView(). Consumer does necessary work.
        /// </summary>
        /// <param name="plotContainer"></param>
        public void Render(ContentView plotContainer)
        {
            //Create the surface
            this._plottingSurface = new SciChartSurface(Android.App.Application.Context);

            //Create the series
            this._series = new XyDataSeries<double, double>();
            this._series.AcceptsUnsortedData = true;

            //Creating the axes
            //AxisTitle = "time [s]",
            this._xAxis = new NumericAxis(Android.App.Application.Context)
            {
                AxisTitlePlacement = AxisTitlePlacement.Bottom,
                DrawMinorGridLines = false,
                DrawMajorGridLines = false,
                DrawMajorBands = false,
                GrowBy = new SciChart.Data.Model.DoubleRange(0.1d, 0.1d)
            };

            this._xAxis.SetAxisTitleMargins(0, 0, 0, 20);

            //AxisTitle = "h(t)",
            this._yAxis = new NumericAxis(Android.App.Application.Context)
            {
                AxisAlignment = AxisAlignment.Left,
                AxisTitlePlacement = AxisTitlePlacement.Left,
                AxisTitleOrientation = AxisTitleOrientation.VerticalFlipped,
                DrawMinorGridLines = false,
                DrawMajorGridLines = false,
                DrawMajorBands = false,
                GrowBy = new SciChart.Data.Model.DoubleRange(0.1d, 0.1d)
            };

            this._yAxis.SetAxisTitleMargins(20, 0, 0, 0);

            //The Renderable Series
            this._renderableSeries = new FastLineRenderableSeries { DataSeries = this._series, StrokeStyle = new SolidPenStyle(0xFF279B27, 2f) };
            EllipsePointMarker pointMarker = new EllipsePointMarker { StrokeStyle = new SolidPenStyle(Android.Graphics.Color.PaleVioletRed, 0.5f), FillStyle = new SolidBrushStyle(0xFFFFA300) };
            pointMarker.SetSize(6.ToDip(Android.App.Application.Context), 6.ToDip(Android.App.Application.Context));
            this._renderableSeries.StrokeStyle = new SolidPenStyle(Android.Graphics.Color.Rgb(255, 64, 129), 2f);

            //Adding this stuff to the surface
            using (this._plottingSurface.SuspendUpdates())
            {
                this._plottingSurface.XAxes.Add(this._xAxis);
                this._plottingSurface.YAxes.Add(this._yAxis);
                this._plottingSurface.RenderableSeries.Add(this._renderableSeries);
                this._plottingSurface.ChartModifiers = new ChartModifierCollection
                {
                    new ZoomPanModifier(),
                    new PinchZoomModifier(),
                    new ZoomExtentsModifier(),
                };
            }

            //Setting the configured native plot as a child of our cross-platform view.
            plotContainer.Content = this._plottingSurface.ToView();
        }

        /// <summary>
        /// Plots all data points at once
        /// </summary>
        /// <param name="dataSeries"></param>
        public void Plot(IEnumerable<IPoint> dataSeries)
        {
            this.StopIfAnimating();
            this.Clear();

            this._series.Append(dataSeries.Select(p => p.X), dataSeries.Select(p => p.Y));
            this._plottingSurface.ZoomExtents();
        }

        /// <summary>
        /// Plots data points over a desired time span. Speed will vary based on device capability.
        /// </summary>
        /// <param name="dataSeries"></param>
        /// <param name="desiredTimeInMillis"></param>
        public void PlotAnimated(IEnumerable<IPoint> dataSeries, double desiredTimeInMillis)
        {
            this.StopIfAnimating();
            this.Clear();

            var dataSeriesEnumerated = dataSeries.ToList();
            double dataBatchSize = dataSeries.Count() / (desiredTimeInMillis / this._animationDelayTime);

            //Do the plotting


            this._plottingSurface.ZoomExtents();
        }

        /// <summary>
        /// Clears the plot
        /// </summary>
        public void Clear()
        {
            this._series.Clear();
        }

        /// <summary>
        /// Sets the X-Axis Title
        /// </summary>
        /// <param name="xAxistTitle"></param>
        public void SetXAxisTitle(string xAxistTitle)
        {
            this._xAxis.AxisTitle = xAxistTitle;
        }

        /// <summary>
        /// Sets the Y-Axis Title
        /// </summary>
        /// <param name="yAxisTitle"></param>
        public void SetYAxisTitle(string yAxisTitle)
        {
            this._yAxis.AxisTitle = yAxisTitle;
        }

        #endregion

        #region Non-Public Methods

        /// <summary>
        /// Stops the current animation timer
        /// </summary>
        private void StopIfAnimating()
        {
            this._animationTimer.Stop();
        }

        #endregion
    }
}