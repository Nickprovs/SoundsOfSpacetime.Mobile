using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Nickprovs.Albatross.Droid.Services;
using SciChart.Charting.Model;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Modifiers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.Common;
using SoundsOfSpacetime.Mobile.Droid.Utilities;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.Types;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(SciChartService_Android))]
namespace Nickprovs.Albatross.Droid.Services
{
    public class SciChartService_Android : IPlotService
    {
        #region Fields

        /// <summary>
        /// The plotting surface
        /// </summary>
        private SciChartSurface _plottingSurface;

        /// <summary>
        /// The data driving series drawn on the plotting surface
        /// </summary>
        private XyDataSeries<double, double> _series;

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
        private PlotAnimationTimer _plotAnimationTimer;

        /// <summary>
        /// The animation delay time when performing animations
        /// </summary>
        private readonly double _animationDelayTime;

        /// <summary>
        /// AnimationLock
        /// </summary>
        private readonly object _plotAnimationLock;

        /// <summary>
        /// The Plot title as a forms label
        /// </summary>
        private Xamarin.Forms.Label _title;

        #endregion

        #region Contructors and Destructors

        /// <summary>
        /// Sets the necessary lifetime data
        /// </summary>
        public SciChartService_Android()
        {
            //We're going to append some number of points and then wait the delay time to give the animation effect
            this._animationDelayTime = 50;

            //This timer will handle all of the waiting between point appendages to give the appearance of animation.
            this._plotAnimationTimer = new PlotAnimationTimer { Interval = this._animationDelayTime };
            this._plotAnimationTimer.Elapsed += OnPlotTimerElapsed;
            this._plotAnimationLock = new object();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the plot and returns it as a Forms View
        /// </summary>
        /// <param name="plotContainer"></param>
        public Xamarin.Forms.View Render()
        {
            Application.Current.Resources.TryGetValue("B7", out var backgroundColorObj);
            var backgroundColorHex = ((Xamarin.Forms.Color)backgroundColorObj).ToHex();

            //Create the surface
            this._plottingSurface = new SciChartSurface(Android.App.Application.Context);
            this._plottingSurface.SetBackgroundColor(Android.Graphics.Color.ParseColor(backgroundColorHex));

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

            //Returns the native plot as a Forms View
            Xamarin.Forms.Grid plotGrid = new Grid { BackgroundColor = Xamarin.Forms.Color.FromHex(backgroundColorHex) };
            var row0 = new RowDefinition { Height = GridLength.Auto };
            var row1 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            plotGrid.RowDefinitions.Add(row0);
            plotGrid.RowDefinitions.Add(row1);

            this._title = new Xamarin.Forms.Label { Text = "Plot", FontSize = 16, TextColor = Xamarin.Forms.Color.White, Margin = new Thickness(0,10,0,0), HorizontalOptions = LayoutOptions.Center};
            plotGrid.Children.Add(this._title);
            this._title.SetValue(Grid.RowProperty, 0);

            var formsPlotView = this._plottingSurface.ToView();
            plotGrid.Children.Add(formsPlotView);
            formsPlotView.SetValue(Grid.RowProperty, 1);

            return plotGrid;
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
            int dataBatchSize = (int)(dataSeries.Count() / (desiredTimeInMillis / this._animationDelayTime));

            //Create a cache for the animated plot as we'll be plotting a few points and then waiting for some time to give the animated effect.
            PlotAnimationCache plotAnimationCache = new PlotAnimationCache(dataSeriesEnumerated, dataBatchSize);
            this._plotAnimationTimer.Cache = plotAnimationCache;
            this._plotAnimationTimer.Start();

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

        public void SetTitle(string title)
        {
            this._title.Text = title;
        }

        #endregion

        #region Non-Public Methods

        /// <summary>
        /// Fascilitates calls necessary to draw the next batch of points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlotTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.DrawNextBatchOfPoints();
        }

        /// <summary>
        /// Draws the next batch of points
        /// </summary>
        private void DrawNextBatchOfPoints()
        {
            lock (this._plotAnimationLock)
            {
                //If the data necessary for drawing the next batch of points isn't valid... return
                if (this._plotAnimationTimer.Cache == null || this._plotAnimationTimer.Cache?.DataSeries == null || this._plotAnimationTimer.Cache?.BatchSize <= 0)
                    return;

                //Get and append the next batch of points
                var nextBatchOfPoints = this._plotAnimationTimer.Cache.DataSeries.GetRange(this._plotAnimationTimer.Cache.Offset, this._plotAnimationTimer.Cache.BatchSize).ToList();
                this._series.Append(nextBatchOfPoints.Select(point => point.X), nextBatchOfPoints.Select(point => point.Y));
                this._plottingSurface.ZoomExtents();
                this._plotAnimationTimer.Cache.Offset = this._plotAnimationTimer.Cache.Offset + this._plotAnimationTimer.Cache.BatchSize;

                if (this._plotAnimationTimer.Cache.Offset >= this._plotAnimationTimer.Cache.DataSeries.Count)
                {
                    this._plotAnimationTimer.Stop();
                    return;
                }
            }

        }

        /// <summary>
        /// Stops the current animation timer
        /// </summary>
        private void StopIfAnimating()
        {
            this._plotAnimationTimer.Stop();
        }

        #endregion
    }
}