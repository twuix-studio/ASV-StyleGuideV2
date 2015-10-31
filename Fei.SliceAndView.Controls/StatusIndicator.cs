using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Fei.SliceAndView.Controls
{
    public enum Status
    {
        Valid,
        ValidGray,
        Invalid,
        Warning,
        Info,
        None
    }

    /// <summary>
    /// Visual representation of correct value
    /// </summary>
    public class StatusIndicator : UserControl
    {
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register( "Status", typeof( Status ), typeof( StatusIndicator ),
            new FrameworkPropertyMetadata( Status.Invalid ) );

        public Status Status
        {
            get { return (Status)GetValue( StatusProperty ); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty TooltipOnNonValidStatusProperty = DependencyProperty.Register( "TooltipOnNonValidStatus", typeof( UIElement ), typeof( StatusIndicator ),
            new FrameworkPropertyMetadata( null ) );

        public UIElement TooltipOnNonValidStatus
        {
            get { return (UIElement)GetValue( TooltipOnNonValidStatusProperty ); }
            set { SetValue( TooltipOnNonValidStatusProperty, value ); }
        }
    }
}
