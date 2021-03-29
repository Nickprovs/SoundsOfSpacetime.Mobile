using SciChart.iOS.Charting;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.iOS.Services;
using SoundsOfSpacetime.Mobile.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(SciChartService_iOS))]
namespace SoundsOfSpacetime.Mobile.iOS.Services
{
    public class SciChartService_iOS : IPlotService
    {
        #region Fields

        /// <summary>
        /// The plotting surface
        /// </summary>
        private SCIChartSurface _plottingSurface;

        /// <summary>
        /// The data driving series drawn on the plotting surface
        /// </summary>
        private XyDataSeries<double, double> _series;

        /// <summary>
        ///The view drawing the data series   
        /// </summary>
        private SCIFastLineRenderableSeries _renderableSeries;

        /// <summary>
        /// The x-axis
        /// </summary>
        SCINumericAxis _xAxis;

        /// <summary>
        /// The y-axis
        /// </summary>
        SCINumericAxis _yAxis;

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

        #region Constructors and Destructors

        /// <summary>
        /// Sets the necessary lifetime data
        /// </summary>
        public SciChartService_iOS()
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

        public void Clear()
        {
            this._series.Clear();
        }

        public void Plot(IEnumerable<IPoint> dataSeries)
        {
            this.StopIfAnimating();
            this.Clear();

            this._series.Append(dataSeries.Select(p => p.X), dataSeries.Select(p => p.Y));
            this._plottingSurface.ZoomExtents();
        }

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

        public View Render()
        {
            Xamarin.Forms.Application.Current.Resources.TryGetValue("B7", out var backgroundColorObj);
            var backgroundColor = ((Xamarin.Forms.Color)backgroundColorObj);

            this._plottingSurface = new SCIChartSurface();
            this._plottingSurface.BackgroundColor = backgroundColor.ToUIColor();
            this._plottingSurface.TranslatesAutoresizingMaskIntoConstraints = true;
            this._series = new XyDataSeries<double, double>();
            this._series.AcceptsUnsortedData = true;

            this._xAxis = new SCINumericAxis 
            {
                AxisTitleAlignment = SCIAlignment.Center,
                AxisTitle = "Time", 
                GrowBy = new SCIDoubleRange(0.1d, 0.1d), 
                DrawMajorGridLines = false,
                DrawMinorGridLines = false,
                DrawMajorBands = false, 
            };

            this._yAxis = new SCINumericAxis 
            {
                AxisTitle = "H(T)",
                AxisTitleAlignment = SCIAlignment.Center,
                GrowBy = new SCIDoubleRange(0.1d, 0.1d),
                DrawMajorGridLines = false,
                DrawMinorGridLines = false,
                DrawMajorBands = false,
            };

            this._renderableSeries = new SCIFastLineRenderableSeries { DataSeries = this._series, StrokeStyle = new SCISolidPenStyle(0xFF279B27, 2f) };
            this._renderableSeries.StrokeStyle = new SCISolidPenStyle(UIColor.FromRGB(255, 64, 129), 2f);
            using (this._plottingSurface.SuspendUpdates())
            {
                this._plottingSurface.XAxes.Add(this._xAxis);
                this._plottingSurface.YAxes.Add(this._yAxis);
                this._plottingSurface.RenderableSeries.Add(this._renderableSeries);
                this._plottingSurface.ChartModifiers = new SCIChartModifierCollection
                {
                    new SCIZoomPanModifier(),
                    new SCIPinchZoomModifier(),
                    new SCIZoomExtentsModifier(),
                };
            }

            //Returns the native plot as a Forms View
            Xamarin.Forms.Grid plotGrid = new Grid { BackgroundColor = Xamarin.Forms.Color.FromHex(backgroundColor.ToHex()) };
            var row0 = new RowDefinition { Height = GridLength.Auto };
            var row1 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            plotGrid.RowDefinitions.Add(row0);
            plotGrid.RowDefinitions.Add(row1);

            this._title = new Xamarin.Forms.Label { Text = "Plot", TextColor = Xamarin.Forms.Color.White, FontSize = 16, Margin = new Thickness(0, 10, 0, 0), HorizontalOptions = LayoutOptions.Center };
            plotGrid.Children.Add(this._title);
            this._title.SetValue(Grid.RowProperty, 0);

            var formsPlotView = this._plottingSurface.ToView();
            plotGrid.Children.Add(formsPlotView);
            formsPlotView.SetValue(Grid.RowProperty, 1);

            return plotGrid;
        }

        public void SetTitle(string title)
        {
            this._title.Text = title;
        }

        public void SetXAxisTitle(string xAxistTitle)
        {
            this._xAxis.AxisTitle = xAxistTitle;
        }   


        public void SetYAxisTitle(string yAxisTitle)
        {
            this._yAxis.AxisTitle = yAxisTitle;
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