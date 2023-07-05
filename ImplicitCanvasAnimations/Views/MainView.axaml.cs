using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Rendering;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace ImplicitCanvasAnimations.Views;

public partial class MainView : UserControl
{
    private ImplicitAnimationCollection? _implicitAnimations;
    private IDisposable? _disposable;

    public MainView()
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

        var sprintEasing1 = new SpringEasing(1.5, 2000, 20);
        var sprintEasing2 = new SpringEasing(1, 1000, 20);

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

    private void Add()
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

    private void Benchmark()
    {
        if (_disposable is null)
        {
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(50))
                .Subscribe(_ =>
                {
                    Dispatcher.UIThread.Post(Add);
                });

            ButtonBenchmark.Content = "Stop";
        }
        else
        {
            _disposable?.Dispose();
            _disposable = null;

            ButtonBenchmark.Content = "Benchmark";
        }
    }
    private void Clear()
    {
        Canvas.Children.Clear();
    }

    private void ButtonAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        Add();
    }

    private void ButtonBenchmark_OnClick(object? sender, RoutedEventArgs e)
    {
        Benchmark();
    }

    private void ButtonClear_OnClick(object? sender, RoutedEventArgs e)
    {
        Clear();
    }

    private void ButtonFps_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.GetVisualRoot() is TopLevel topLevel)
        {
            topLevel.RendererDiagnostics.DebugOverlays = 
                topLevel.RendererDiagnostics.DebugOverlays == RendererDebugOverlays.Fps 
                    ? RendererDebugOverlays.None 
                    : RendererDebugOverlays.Fps;
        }
    }
}
