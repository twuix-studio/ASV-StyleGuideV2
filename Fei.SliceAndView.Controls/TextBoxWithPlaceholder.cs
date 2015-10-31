using System.Windows;
using System.Windows.Controls;

namespace Fei.SliceAndView.Controls
{
    public class TextBoxWithPlaceholder : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(object), typeof(TextBoxWithPlaceholder));

        public static readonly DependencyProperty IsPlaceholderVisibleProperty =
            DependencyProperty.Register("IsPlaceholderVisible", typeof(bool), typeof(TextBoxWithPlaceholder));

        public object Placeholder
        {
            get { return GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public bool IsPlaceholderVisible
        {
            get { return (bool)GetValue(IsPlaceholderVisibleProperty); }
            private set { SetValue(IsPlaceholderVisibleProperty, value); }
        }

        #region Constructors

        public TextBoxWithPlaceholder()
        {
            UpdatePlaceholderVisibility();
        }

        #endregion

        #region Overrides of TextBoxBase

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdatePlaceholderVisibility();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdatePlaceholderVisibility();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            UpdatePlaceholderVisibility();
        }

        #endregion

        private void UpdatePlaceholderVisibility()
        {
            if (!this.IsFocused && this.Text.Length == 0)
            {
                IsPlaceholderVisible = true;
            }
            else
            {
                IsPlaceholderVisible = false;
            }
        }
    }
}
