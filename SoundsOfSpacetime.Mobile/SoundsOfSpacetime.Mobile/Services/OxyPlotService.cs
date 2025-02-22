﻿using System.Collections.Generic;
using System.Linq;
using System.Timers;
using SoundsOfSpacetime.Mobile.Interfaces;
using SoundsOfSpacetime.Mobile.Services;
using SoundsOfSpacetime.Mobile.Types;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(OxyPlotService))]
namespace SoundsOfSpacetime.Mobile.Services
{
    public class OxyPlotService : IPlotService
    {
        #region Fields

        /// <summary>
        /// The plotting surface
        /// </summary>
        private PlotView _plottingSurface;

        /// <summary>
        /// The plotting model
        /// </summary>
        private PlotModel _plottingModel;

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

        private LineSeries _series;

        private OxyPlot.Axes.LinearAxis _xAxis;

        private OxyPlot.Axes.LinearAxis _yAxis;


        #endregion

        #region Contructors and Destructors

        /// <summary>
        /// Sets the necessary lifetime data
        /// </summary>
        public OxyPlotService()
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
            Application.Current.Resources.TryGetValue("B2", out var b2ColorObj);
            var b2ColorHex = ((Xamarin.Forms.Color)b2ColorObj).ToHex();
            var oxyB2Color = OxyColor.Parse(b2ColorHex);

            Application.Current.Resources.TryGetValue("F7", out var f7ColorObj);
            var f7ColorHex = ((Xamarin.Forms.Color)f7ColorObj).ToHex();
            var oxyF7Color = OxyColor.Parse(f7ColorHex);

            Application.Current.Resources.TryGetValue("F1", out var f1ColorObj);
            var f1ColorHex = ((Xamarin.Forms.Color)f1ColorObj).ToHex();
            var oxyF1Color = OxyColor.Parse(f1ColorHex);

            this._series = new LineSeries
            {
                StrokeThickness = 1,
                LineStyle = LineStyle.Solid,
                Color = oxyF7Color,
                MarkerType = MarkerType.None,
                Background = oxyB2Color,
            };


            this._plottingModel = new PlotModel
            {
                LegendTextColor = oxyF1Color,
                LegendTitleColor = oxyF1Color,
                TextColor = oxyF1Color,
                TitleColor = oxyF1Color
            };
            this._xAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom };
            this._yAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left };
            this._plottingModel.Axes.Add(this._xAxis);
            this._plottingModel.Axes.Add(this._yAxis);

            this._plottingSurface = new PlotView
            {
                Model = this._plottingModel,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromHex(b2ColorHex),
            };

            this._plottingModel.Series.Add(this._series);
            this._plottingSurface.Model = this._plottingModel;
            return this._plottingSurface;
        }

        /// <summary>
        /// Plots all data points at once
        /// </summary>
        /// <param name="dataSeries"></param>
        public void Plot(IEnumerable<IPoint> dataSeries)
        {
            //this.StopIfAnimating();
            //this.Clear();

            //this._series.Append(dataSeries.Select(p => p.X), dataSeries.Select(p => p.Y));
            //this._plottingSurface.ZoomExtents();
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
            this._plottingModel.ResetAllAxes();

            var dataSeriesEnumerated = dataSeries.ToList();
            int dataBatchSize = (int)(dataSeries.Count() / (desiredTimeInMillis / this._animationDelayTime));

            //Create a cache for the animated plot as we'll be plotting a few points and then waiting for some time to give the animated effect.
            PlotAnimationCache plotAnimationCache = new PlotAnimationCache(dataSeriesEnumerated, dataBatchSize);
            this._plotAnimationTimer.Cache = plotAnimationCache;
            this._plotAnimationTimer.Start();



        }

        /// <summary>
        /// Clears the plot
        /// </summary>
        public void Clear()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                this._series.Points.Clear();
                this._plottingModel.InvalidatePlot(true);
            });
        }

        /// <summary>
        /// Sets the X-Axis Title
        /// </summary>
        /// <param name="xAxistTitle"></param>
        public void SetXAxisTitle(string xAxistTitle)
        {
            this._xAxis.Title = xAxistTitle;
        }

        /// <summary>
        /// Sets the Y-Axis Title
        /// </summary>
        /// <param name="yAxisTitle"></param>
        public void SetYAxisTitle(string yAxisTitle)
        {
            this._yAxis.Title = yAxisTitle;
        }

        /// <summary>
        /// Sets the title of the graph
        /// </summary>
        /// <param name="title"></param>
        public void SetTitle(string title)
        {
            this._plottingModel.Title = title;
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

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this._series.Points.AddRange(nextBatchOfPoints.Select(point => new DataPoint(point.X, point.Y)));
                    this._plottingModel.InvalidatePlot(true);
                });

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
            lock (this._plotAnimationLock)
            {
                this._plotAnimationTimer.Stop();
            }
        }

        #endregion
    }
}