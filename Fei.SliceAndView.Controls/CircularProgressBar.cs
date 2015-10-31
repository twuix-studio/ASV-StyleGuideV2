using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fei.SliceAndView.Controls
{
    public class CircularProgressBar : Canvas
    {
        private readonly DispatcherTimer animationTimer;
        private List<Ellipse> dots;

        public static readonly DependencyProperty DotBrushProperty = DependencyProperty.Register(
            "DotBrush", typeof(Brush), typeof(CircularProgressBar), new PropertyMetadata(new SolidColorBrush(Colors.WhiteSmoke), OnDotsChanged));

        public Brush DotBrush
        {
            get { return (Brush)GetValue(DotBrushProperty); }
            set { SetValue(DotBrushProperty, value); }
        }

        public static readonly DependencyProperty DotsCountProperty = DependencyProperty.Register(
            "DotsCount", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(10, OnDotsChanged));

        public int DotsCount
        {
            get { return (int)GetValue(DotsCountProperty); }
            set { SetValue(DotsCountProperty, value); }
        }

        public static readonly DependencyProperty DotSizeProperty = DependencyProperty.Register(
            "DotSize", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(10.0, OnDotsChanged));

        public double DotSize
        {
            get { return (double)GetValue(DotSizeProperty); }
            set { SetValue(DotSizeProperty, value); }
        }

        public CircularProgressBar()
        {
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
            this.IsVisibleChanged += OnVisibleChanged;
            this.SizeChanged += OnSizeChanged;
            this.RenderTransform = new RotateTransform();

            animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }

        private static void OnDotsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularProgressBar circularProgress = (CircularProgressBar)d;
            circularProgress.CreateDots();
            circularProgress.PlaceDots();
        }

        private void Start()
        {
            if (!animationTimer.IsEnabled)
            {
                CreateDots();
                PlaceDots();

                animationTimer.Tick += HandleAnimationTick;
                animationTimer.Start();
            }
        }

        private void Stop()
        {
            if (animationTimer.IsEnabled)
            {
                animationTimer.Tick -= HandleAnimationTick;
                animationTimer.Stop();
            }
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            RotateTransform rotation = this.RenderTransform as RotateTransform;
            if (rotation != null)
            {
                rotation.CenterX = ActualWidth / 2.0;
                rotation.CenterY = ActualHeight / 2.0;
                rotation.Angle = (rotation.Angle + 36) % 360;
            }
        }

        private void CreateDots()
        {
            if (dots != null)
            {
                foreach (var dot in this.dots)
                {
                    this.Children.Remove(dot);
                }
            }

            this.dots = new List<Ellipse>();
            double opacity = 0;
            double opacityStep = 1.0 / DotsCount;
            for (int i = 0; i < DotsCount; i++)
            {
                Ellipse dot = new Ellipse()
                {
                    Width = DotSize,
                    Height = DotSize,
                    Fill = DotBrush,
                    Opacity = opacity
                };

                this.Children.Add(dot);
                this.dots.Insert(0, dot);

                opacity += opacityStep;
            }
        }

        private void PlaceDots()
        {
            double halfSize = (Math.Min(ActualWidth, ActualHeight) - DotSize) / 2.0;
            double offset = Math.PI;
            double offsetStep = 2.0 * Math.PI / DotsCount;

            foreach (var dot in this.dots)
            {
                dot.SetValue(Canvas.LeftProperty, halfSize + Math.Sin(offset) * (halfSize - DotSize / 2.0));
                dot.SetValue(Canvas.TopProperty, halfSize + Math.Cos(offset) * (halfSize - DotSize / 2.0));

                offset += offsetStep;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PlaceDots();
        }
    }
}