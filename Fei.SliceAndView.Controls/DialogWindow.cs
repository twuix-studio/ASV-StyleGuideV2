using System;
using System.Windows;
using System.Windows.Input;

namespace Fei.SliceAndView.Controls
{
    public class DialogWindow : Window
    {
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
