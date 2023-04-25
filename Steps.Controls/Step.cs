using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Steps.Controls;

public class Step : Control
{
    private const double AnimateSize = 10;

    private static double _stepOuterBorderHeight;
    private static double _stepOuterBorderWidth;

    public double StepLineSize
    {
        get => (double)GetValue(StepLineSizeProperty);
        set => SetValue(StepLineSizeProperty, value);
    }

    public static readonly DependencyProperty StepLineSizeProperty =
        DependencyProperty.Register(nameof(StepLineSize), typeof(double), typeof(Step),
            new PropertyMetadata(50.0));


    public double StepHeight
    {
        get => (double)GetValue(StepHeightProperty);
        set => SetValue(StepHeightProperty, value);
    }

    public static readonly DependencyProperty StepHeightProperty =
        DependencyProperty.Register(nameof(StepHeight), typeof(double), typeof(Step),
            new PropertyMetadata(0.0, StepHeightPropChanged));

    private static void StepHeightPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Animate(_partOuterBorder, Border.HeightProperty, _stepOuterBorderHeight,
            Convert.ToDouble(e.NewValue) + AnimateSize);
        _stepOuterBorderHeight = Convert.ToDouble(e.NewValue) + AnimateSize;
    }

    public double StepWidth
    {
        get => (double)GetValue(StepWidthProperty);
        set => SetValue(StepWidthProperty, value);
    }

    public static readonly DependencyProperty StepWidthProperty =
        DependencyProperty.Register(nameof(StepWidth), typeof(double), typeof(Step),
            new PropertyMetadata(0.0, StepWidthPropChanged));

    private static void StepWidthPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Animate(_partOuterBorder, Border.WidthProperty, _stepOuterBorderWidth,
            Convert.ToDouble(e.NewValue) + AnimateSize);
        _stepOuterBorderWidth = Convert.ToDouble(e.NewValue) + AnimateSize;
    }


    public CornerRadius StepRoundness
    {
        get => (CornerRadius)GetValue(StepRoundnessProperty);
        set => SetValue(StepRoundnessProperty, value);
    }

    public static readonly DependencyProperty StepRoundnessProperty =
        DependencyProperty.Register(nameof(StepRoundness), typeof(CornerRadius), typeof(Step),
            new PropertyMetadata(new CornerRadius(100)));

    public Orientation StepOrientation
    {
        get => (Orientation)GetValue(StepOrientationProperty);
        set => SetValue(StepOrientationProperty, value);
    }

    public static readonly DependencyProperty StepOrientationProperty =
        DependencyProperty.Register(nameof(StepOrientation), typeof(Orientation), typeof(Step),
            new PropertyMetadata(Orientation.Horizontal));


    public bool StepIsCompleted
    {
        get => (bool)GetValue(StepIsCompletedProperty);
        set => SetValue(StepIsCompletedProperty, value);
    }

    public static readonly DependencyProperty StepIsCompletedProperty =
        DependencyProperty.Register(nameof(StepIsCompleted), typeof(bool), typeof(Step),
            new PropertyMetadata(false));


    public bool StepLineVisibility
    {
        get => (bool)GetValue(StepLineVisibilityProperty);
        set => SetValue(StepLineVisibilityProperty, value);
    }

    // Using a DependencyProperty as the backing store for StepLineVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StepLineVisibilityProperty =
        DependencyProperty.Register(nameof(StepLineVisibility), typeof(bool), typeof(Step),
            new PropertyMetadata(true));


    public object StepContent
    {
        get => GetValue(StepContentProperty);
        set => SetValue(StepContentProperty, value);
    }

    public static readonly DependencyProperty StepContentProperty =
        DependencyProperty.Register(nameof(StepContent), typeof(object), typeof(Step),
            new PropertyMetadata(null));

    private static Border? _partInnerBorder;

    private static Border? _partOuterBorder;
    //----------------------------------------------------CODE------------------------------------------------------------

    static Step()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Step), new FrameworkPropertyMetadata(typeof(Step)));
    }

    public override void OnApplyTemplate()
    {
        _partInnerBorder = (Template.FindName("PART_InnerBorder", this) as Border)!;
        _partOuterBorder = (Template.FindName("PART_OuterBorder", this) as Border)!;

        _partOuterBorder.Width = _stepOuterBorderWidth;
        _partOuterBorder.Height = _stepOuterBorderHeight;


        _partInnerBorder.MouseEnter += InnerBorder_OnMouseEnter;
        _partInnerBorder.MouseLeave += InnerBorder_OnMouseLeave;
        base.OnApplyTemplate();
    }


    private static void Animate(Border? border, DependencyProperty dp, double from, double to)
    {
        var anim = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = new Duration(TimeSpan.FromMilliseconds(700)),
            EasingFunction = new CircleEase(),
        };

        border?.BeginAnimation(dp, anim);
    }

    private void InnerBorder_OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (!StepIsCompleted)
        {
            Animate(_partOuterBorder, Border.HeightProperty, _stepOuterBorderHeight,
                _stepOuterBorderHeight + AnimateSize);
            Animate(_partOuterBorder, Border.WidthProperty, _stepOuterBorderWidth,
                _stepOuterBorderWidth + AnimateSize);
        }

        e.Handled = true;
    }

    private void InnerBorder_OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (!StepIsCompleted)
        {
            Animate(_partOuterBorder, Border.HeightProperty, _stepOuterBorderHeight + AnimateSize,
                _stepOuterBorderHeight);
            Animate(_partOuterBorder, Border.WidthProperty, _stepOuterBorderWidth + AnimateSize,
                _stepOuterBorderWidth);
        }

        e.Handled = true;
    }
}