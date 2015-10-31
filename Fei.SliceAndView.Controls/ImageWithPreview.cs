using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Fei.SliceAndView.Controls
{
    /// <summary>
    /// Control designed to display image from a list of images. It uses a list of previews to fastly display an image and a list of files to load a full-resolution image.
    /// </summary>
    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class ImageWithPreview : Control
    {
        /// <summary>
        /// Delay between the current image is selected and high-resolution image is loading is started. 
        /// This delay is important to ensure fluent selected image changes.
        /// </summary>
        private const double LOAD_HIQUALITY_DELAY = 500.0;       // 500ms

        private Image image;
        private string selectedSource;

        /// <summary>
        /// Timer used for delayed loading of full quality image.
        /// </summary>
        private readonly System.Timers.Timer loadDetailTimer;

        public static readonly DependencyProperty SourcesProperty = DependencyProperty.Register(
            "Sources", typeof(IList<string>), typeof(ImageWithPreview), new PropertyMetadata(null, OnSelectedIndexChanged));

        /// <summary>
        /// List of paths to original image files.
        /// </summary>
        public IList<string> Sources
        {
            get { return (IList<string>)GetValue(SourcesProperty); }
            set { SetValue(SourcesProperty, value); }
        }

        public static readonly DependencyProperty PreviewsProperty = DependencyProperty.Register(
            "Previews", typeof(IList<BitmapSource>), typeof(ImageWithPreview), new PropertyMetadata(null, OnSelectedIndexChanged));

        /// <summary>
        /// List of preview images.
        /// </summary>
        public IList<BitmapSource> Previews
        {
            get { return (IList<BitmapSource>)GetValue(PreviewsProperty); }
            set { SetValue(PreviewsProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(ImageWithPreview), new PropertyMetadata(0, OnSelectedIndexChanged));

        /// <summary>
        /// Currently displayed image index.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static readonly DependencyPropertyKey IsLoadingDetailPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsLoadingDetail", typeof(bool), typeof(ImageWithPreview), new PropertyMetadata(false));

        public static DependencyProperty IsLoadingDetailProperty = IsLoadingDetailPropertyKey.DependencyProperty;

        /// <summary>
        /// Indicates whether the full-resolution image is loading.
        /// </summary>
        public bool IsLoadingDetail
        {
            get { return (bool)GetValue(IsLoadingDetailProperty); }
            private set { SetValue(IsLoadingDetailPropertyKey, value); }
        }

        #region Constructors

        public ImageWithPreview()
        {
            loadDetailTimer = new System.Timers.Timer()
            {
                AutoReset = false,
                Interval = LOAD_HIQUALITY_DELAY
            };
            loadDetailTimer.Elapsed += LoadDetail;
        }

        #endregion
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.Template != null)
            {
                Image newImage = this.Template.FindName("PART_Image", this) as Image;
                if (newImage != this.image)
                {
                    this.image = newImage;
                }

                UpdateImage();
            }
        }
        
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageWithPreview viewer = (ImageWithPreview)d;
            viewer.UpdateImage();
        }

        /// <summary>
        /// Update image source according to selected index.
        /// Stops currently pending delayed loading of high-quality image and sets a new one.
        /// </summary>
        private void UpdateImage()
        {
            if (this.image == null)
            {
                return;
            }

            int newIndex = this.SelectedIndex;
            if (newIndex >= 0)
            {
                if (this.Previews != null && newIndex < this.Previews.Count)
                {
                    this.image.Source = this.Previews[newIndex];
                }
                else
                {
                    this.image.Source = null;
                }

                // stop pending loading of a high-quality image
                this.loadDetailTimer.Stop();
                if (this.Sources != null && newIndex < this.Sources.Count)
                {
                    this.selectedSource = this.Sources[newIndex];

                    // set loading of a high-quality image pending
                    this.loadDetailTimer.Start();
                    this.IsLoadingDetail = true;
                }
                else
                {
                    this.selectedSource = null;
                    this.IsLoadingDetail = false;
                }
            }
        }

        /// <summary>
        /// Loads a full-quality image from appropriate source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void LoadDetail(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (this.selectedSource != null)
            {
                string path = this.selectedSource;
                BitmapImage bitmapSource = new BitmapImage();
                bitmapSource.BeginInit();
                bitmapSource.UriSource = new Uri(path);
                bitmapSource.CacheOption = BitmapCacheOption.OnLoad;
                bitmapSource.EndInit();
                bitmapSource.Freeze();

                Dispatcher.BeginInvoke(new Action<string, BitmapImage>((bitmapPath, bitmap) =>
                                                                       {
                                                                           // do not load an image when its path do not correspond to currently selected path
                                                                           // 
                                                                           if(this.Sources != null && this.image != null && 
                                                                               this.SelectedIndex >= 0 && this.SelectedIndex < this.Sources.Count &&
                                                                               this.Sources[this.SelectedIndex] == bitmapPath)
                                                                           {
                                                                               this.image.Source = bitmap;
                                                                               this.IsLoadingDetail = false;
                                                                           }
                                                                       }), path, bitmapSource);
            }

        }
    }
}
