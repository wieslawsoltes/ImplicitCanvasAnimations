using System;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;

namespace CatchMeIfYouCan.Views;

public partial class MainWindow : Window
{
    private ImplicitAnimationCollection? _implicitAnimations;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void EnsureImplicitAnimations()
    {
        if (_implicitAnimations != null)
        {
            return;
        }

        var compositor = ElementComposition.GetElementVisual(this)!.Compositor;

        var sprintEasing1 = new SpringEasing(1.5, 2000, 20, 0);
        var sprintEasing2 = new SpringEasing(1, 1000, 20, 0);

        var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
        offsetAnimation.Target = "Offset";
        offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", sprintEasing1);
        offsetAnimation.Duration = TimeSpan.FromMilliseconds(400);

        var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
        rotationAnimation.Target = "RotationAngle";
        rotationAnimation.InsertKeyFrame(0.0f, 0.0f, sprintEasing2);
        rotationAnimation.InsertKeyFrame(1.0f, (float)(Math.PI * 2.0), sprintEasing2);
        rotationAnimation.Duration = TimeSpan.FromMilliseconds(400);

        var animationGroup = compositor.CreateAnimationGroup();
        animationGroup.Add(offsetAnimation);
        animationGroup.Add(rotationAnimation);

        _implicitAnimations = compositor.CreateImplicitAnimationCollection();
        _implicitAnimations["Offset"] = animationGroup;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var canvasItem = new CanvasItem();

        var left = Random.Shared.NextDouble() * Canvas.Bounds.Width;
        var top = Random.Shared.NextDouble() * Canvas.Bounds.Height;
        
        Canvas.SetLeft(canvasItem, left);
        Canvas.SetTop(canvasItem, top);

        canvasItem.AttachedToVisualTree += (_, _) =>
        {
            EnsureImplicitAnimations();

            if (ElementComposition.GetElementVisual(canvasItem) is { } compositionVisual)
            {
                compositionVisual.ImplicitAnimations = _implicitAnimations;
            }
        };
        
        Canvas.Children.Add(canvasItem);
    }
}
