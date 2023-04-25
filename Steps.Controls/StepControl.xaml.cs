using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Steps.Controls
{
    /// <summary>
    /// Interaction logic for StepControl.xaml
    /// </summary>
    public partial class StepControl : UserControl, INotifyPropertyChanged
    {
        private const double AnimateSize = 10;
        public static double StepOuterBorderWidth { get; set; }
        public static double StepOuterBorderHeight { get; set; }

        public double StepLineSize
        {
            get => (double)GetValue(StepLineSizeProperty);
            set => SetValue(StepLineSizeProperty, value);
        }

        public static readonly DependencyProperty StepLineSizeProperty =
            DependencyProperty.Register(nameof(StepLineSize), typeof(double), typeof(StepControl),
                new PropertyMetadata(50.0));


        public double StepHeight
        {
            get => (double)GetValue(StepHeightProperty);
            set => SetValue(StepHeightProperty, value);
        }

        public static readonly DependencyProperty StepHeightProperty =
            DependencyProperty.Register(nameof(StepHeight), typeof(double), typeof(StepControl),
                new PropertyMetadata(0.0, StepHeightPropChanged));

        private static void StepHeightPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StepControl control)
            {
                Animate(control.OuterBorder, Border.HeightProperty, StepOuterBorderHeight,
                    Convert.ToDouble(e.NewValue) + AnimateSize);
                StepOuterBorderHeight = Convert.ToDouble(e.NewValue) + AnimateSize;
            }
        }

        public double StepWidth
        {
            get => (double)GetValue(StepWidthProperty);
            set => SetValue(StepWidthProperty, value);
        }

        public static readonly DependencyProperty StepWidthProperty =
            DependencyProperty.Register(nameof(StepWidth), typeof(double), typeof(StepControl),
                new PropertyMetadata(0.0, StepWidthPropChanged));

        private static void StepWidthPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StepControl control)
            {
                Animate(control.OuterBorder, Border.WidthProperty, StepOuterBorderWidth,
                    Convert.ToDouble(e.NewValue) + AnimateSize);
                StepOuterBorderWidth = Convert.ToDouble(e.NewValue) + AnimateSize;
            }
        }


        public CornerRadius StepRoundness
        {
            get => (CornerRadius)GetValue(StepRoundnessProperty);
            set => SetValue(StepRoundnessProperty, value);
        }

        public static readonly DependencyProperty StepRoundnessProperty =
            DependencyProperty.Register(nameof(StepRoundness), typeof(CornerRadius), typeof(StepControl),
                new PropertyMetadata(new CornerRadius(100)));

        public Orientation StepOrientation
        {
            get => (Orientation)GetValue(StepOrientationProperty);
            set => SetValue(StepOrientationProperty, value);
        }

        public static readonly DependencyProperty StepOrientationProperty =
            DependencyProperty.Register(nameof(StepOrientation), typeof(Orientation), typeof(StepControl),
                new PropertyMetadata(Orientation.Horizontal));


        public bool StepIsCompleted
        {
            get => (bool)GetValue(StepIsCompletedProperty);
            set => SetValue(StepIsCompletedProperty, value);
        }

        public static readonly DependencyProperty StepIsCompletedProperty =
            DependencyProperty.Register(nameof(StepIsCompleted), typeof(bool), typeof(StepControl),
                new PropertyMetadata(false));


        public bool StepLineVisibility
        {
            get => (bool)GetValue(StepLineVisibilityProperty);
            set => SetValue(StepLineVisibilityProperty, value);
        }

        // Using a DependencyProperty as the backing store for StepLineVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepLineVisibilityProperty =
            DependencyProperty.Register(nameof(StepLineVisibility), typeof(bool), typeof(StepControl),
                new PropertyMetadata(true));


        public object StepContent
        {
            get => (object)GetValue(StepContentProperty);
            set => SetValue(StepContentProperty, value);
        }

        public static readonly DependencyProperty StepContentProperty =
            DependencyProperty.Register(nameof(StepContent), typeof(object), typeof(StepControl),
                new PropertyMetadata(null));


        public StepControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region INotify Property

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        private static void Animate(IAnimatable border, DependencyProperty dp, double from, double to)
        {
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(700)),
                EasingFunction = new CircleEase(),
            };

            border.BeginAnimation(dp, anim);
        }

        private void InnerBorder_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!StepIsCompleted)
            {
                Animate(OuterBorder, Border.HeightProperty, StepOuterBorderHeight, StepOuterBorderHeight + AnimateSize);
                Animate(OuterBorder, Border.WidthProperty, StepOuterBorderWidth, StepOuterBorderWidth + AnimateSize);
            }
        }

        private void InnerBorder_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!StepIsCompleted)
            {
                Animate(OuterBorder, Border.HeightProperty, StepOuterBorderHeight + AnimateSize, StepOuterBorderHeight);
                Animate(OuterBorder, Border.WidthProperty, StepOuterBorderWidth + AnimateSize, StepOuterBorderWidth);
            }
        }
    }
}