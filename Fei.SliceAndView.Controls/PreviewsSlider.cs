using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fei.SliceAndView.Controls
{
    [TemplatePart(Name = "PART_Previews", Type = typeof(ItemsControl))]
    public class PreviewsSlider : Slider
    {
        private Track track;
        private ItemsControl previews;
        private readonly ObservableCollection<BitmapSource> previewImages;
        
        private static readonly DependencyPropertyKey SelectedIndexPropertyKey = DependencyProperty.RegisterReadOnly(
            "SelectedIndex", typeof(int), typeof(PreviewsSlider), new PropertyMetadata(default(int)));

        public static DependencyProperty SelectedIndexProperty = SelectedIndexPropertyKey.DependencyProperty;

        /// <summary>
        /// Index of the selected image in <see cref="ImageFiles"/>
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            private set { SetValue(SelectedIndexPropertyKey, value); }
        }
        
        public static readonly DependencyProperty ImageFilesProperty = DependencyProperty.Register(
            "ImageFiles", typeof(IList<string>), typeof(PreviewsSlider), new PropertyMetadata(null, OnImagesChanged));

        /// <summary>
        /// List of paths to listed images
        /// </summary>
        public IList<string> ImageFiles
        {
            get { return (IList<string>)GetValue(ImageFilesProperty); }
            set { SetValue(ImageFilesProperty, value); }
        }

        public static readonly DependencyProperty DesiredCountProperty = DependencyProperty.Register(
            "DesiredCount", typeof(int), typeof(PreviewsSlider), new PropertyMetadata(0, OnDesiredCountChanged));

        /// <summary>
        /// Expected number of images
        /// </summary>
        public int DesiredCount
        {
            get { return (int)GetValue(DesiredCountProperty); }
            set { SetValue(DesiredCountProperty, value); }
        }

        private static readonly DependencyPropertyKey LoadedCountPropertyKey = DependencyProperty.RegisterReadOnly(
            "LoadedCount", typeof(int), typeof(PreviewsSlider), new PropertyMetadata(0, OnLoadedCountChanged));

        public static DependencyProperty LoadedCountProperty = LoadedCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Number of images that are available
        /// </summary>
        public int LoadedCount
        {
            get { return (int)GetValue(LoadedCountProperty); }
            private set { SetValue(LoadedCountPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey PendingCountPropertyKey = DependencyProperty.RegisterReadOnly(
            "PendingCount", typeof(int), typeof(PreviewsSlider), new PropertyMetadata(default(int)));

        public static DependencyProperty PendingCountProperty = PendingCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Number of images that are not loaded yet
        /// </summary>
        public int PendingCount
        {
            get { return (int)GetValue(PendingCountProperty); }
            private set { SetValue(PendingCountPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsLoadingPreviewsPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsLoadingPreviews", typeof(bool), typeof(PreviewsSlider), new PropertyMetadata(default(bool)));

        public static DependencyProperty IsLoadingPreviewsProperty = IsLoadingPreviewsPropertyKey.DependencyProperty;

        /// <summary>
        /// Indicates whether previews are loading
        /// </summary>
        public bool IsLoadingPreviews
        {
            get { return (bool)GetValue(IsLoadingPreviewsProperty); }
            private set { SetValue(IsLoadingPreviewsPropertyKey, value); }
        }

        #region Constructors

        public PreviewsSlider()
        {
            this.previewImages = new ObservableCollection<BitmapSource>();
            this.ValueChanged += PreviewsSlider_ValueChanged;
        }

        #endregion
        
        /// <summary>
        /// On slider value change, recompute selected index value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSelectedIndex();
        }

        private void UpdateSelectedIndex()
        {
            int i = Convert.ToInt32(Value);
            if (this.ImageFiles != null && i >= 0 && i < this.ImageFiles.Count)
            {
                this.SelectedIndex = i - 1;
            }
            else
            {
                this.SelectedIndex = -1;
            }
        }

        private static void OnImagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PreviewsSlider slider = (PreviewsSlider)d;

            if (e.OldValue is ObservableCollection<string>)
            {
                (e.OldValue as ObservableCollection<string>).CollectionChanged -= slider.OnImagesCollectionChanged;
            }

            if (e.NewValue is ObservableCollection<string>)
            {
                (e.NewValue as ObservableCollection<string>).CollectionChanged += slider.OnImagesCollectionChanged;
            }

            slider.UpdateProgress();
        }

        private static void OnDesiredCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PreviewsSlider slider = (PreviewsSlider)d;
            slider.UpdateProgress();
        }

        private static void OnLoadedCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PreviewsSlider slider = (PreviewsSlider)d;
            slider.UpdateProgress();
        }

        private void OnImagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateProgress();
        }

        /// <summary>
        /// Find important parts of the template and bind some events.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Template != null)
            {
                ItemsControl newPreviews = this.Template.FindName("PART_Previews", this) as ItemsControl;
                if (newPreviews != this.previews)
                {
                    if (this.previews != null)
                    {
                        this.previews.SizeChanged -= PreviewsSizeChanged;
                    }
                    this.previews = newPreviews;
                    if (this.previews != null)
                    {
                        this.previews.SizeChanged += PreviewsSizeChanged;
                        this.previews.ItemsSource = this.previewImages;

                        RecreatePreviews();
                    }
                }

                Track newTrack = this.Template.FindName("PART_Track", this) as Track;
                if (newTrack != this.track)
                {
                    if (this.track != null)
                    {
                        this.track.Thumb.MouseEnter -= Thumb_MouseEnter;
                    }
                    this.track = newTrack;
                    if (this.track != null)
                    {
                        this.track.Thumb.MouseEnter += Thumb_MouseEnter;
                    }
                }
            }
        }

        /// <summary>
        /// Implements thumb dragging even when you click everywhere on the track, not just into thumb.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumb_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null)
            {
                // the left button is pressed on mouse enter
                // but the mouse isn't captured, so the thumb
                // must have been moved under the mouse in response
                // to a click on the track.
                // Generate a MouseLeftButtonDown event.

                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;

                (sender as Thumb).RaiseEvent(args);
            }
        }

        /// <summary>
        /// When area for previews changed, we need to choose new previews and regenerate them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewsSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecreatePreviews();
        }

        private void RecreatePreviews()
        {
            this.previewImages.Clear();
            if (this.ImageFiles != null && this.previews != null && this.previews.ActualWidth > 0.0)
            {
                Task.Factory.StartNew(UpdatePreviews);
            }
        }

        /// <summary>
        /// Check if a new preview should be displayed for currently loaded images and if yes - load it.
        /// </summary>
        private void UpdatePreviews()
        {
            Dispatcher.Invoke(new Action(() => IsLoadingPreviews = true));

            string path = Dispatcher.Invoke(new Func<string>(GetNextPreviewPath)) as string;
            while (path != null)
            {
                this.LoadPreview(path);
                path = Dispatcher.Invoke(new Func<string>(GetNextPreviewPath)) as string;
            }

            Dispatcher.Invoke(new Action(() => IsLoadingPreviews = false));
        }

        /// <summary>
        /// Get index of a next image that should be displayed as preview.
        /// </summary>
        /// <returns></returns>
        private string GetNextPreviewPath()
        {
            double totalWidth = this.previewImages.Sum(i => i.Width);
            int index = Convert.ToInt32((totalWidth / this.previews.ActualWidth) * this.ImageFiles.Count);
            if (index < this.ImageFiles.Count)
            {
                return this.ImageFiles[index];
            }

            return null;
        }

        private void LoadPreview(object arg)
        {
            string path = (string)arg;
            int desiredHeight = Convert.ToInt32(this.previews.ActualHeight);
            BitmapImage bitmapSource = new BitmapImage();
            bitmapSource.BeginInit();
            bitmapSource.UriSource = new Uri(path);
            bitmapSource.DecodePixelHeight = desiredHeight;
            bitmapSource.CacheOption = BitmapCacheOption.OnLoad;
            bitmapSource.EndInit();
            bitmapSource.Freeze();

            Dispatcher.BeginInvoke(new Action(() => this.previewImages.Add(bitmapSource)));
        }

        /// <summary>
        /// Update LoadedCount, PendingCount and dependent values according to items in <see cref="ImageFiles"/>.
        /// </summary>
        private void UpdateProgress()
        {
            if (this.ImageFiles == null)
            {
                LoadedCount = 0;
            }
            else
            {
                LoadedCount = this.ImageFiles.Count;
                if (SelectedIndex < 1 && LoadedCount > 0)
                {
                    UpdateSelectedIndex();
                }
            }
            
            PendingCount = DesiredCount - LoadedCount;
        }
    }
}
