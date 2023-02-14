using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace ImplicitCanvasAnimations.Views;

public partial class CanvasItem : UserControl
{
    private IDisposable? _disposable;

    public CanvasItem()
    {
        InitializeComponent();

        PointerPressed += (_, _) =>
        {
            Border.Background = Brushes.Green;
            TextBlock.Text = "Stop";
            _disposable?.Dispose();
        };

        AttachedToVisualTree += (_, _) =>
        {
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    Dispatcher.UIThread.Post(Move);
                });
        };

        DetachedFromVisualTree += (_, _) =>
        {
            _disposable?.Dispose();
        };
    }

    private void Move()
    {
        if (Parent is Canvas canvas)
        {
            var left = Random.Shared.NextDouble() * canvas.Bounds.Width;
            var top = Random.Shared.NextDouble() * canvas.Bounds.Height;

            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
    }
}
