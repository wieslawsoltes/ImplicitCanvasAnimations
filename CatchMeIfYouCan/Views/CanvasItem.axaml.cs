using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ReactiveUI;

namespace CatchMeIfYouCan.Views;

public partial class CanvasItem : UserControl
{
    private readonly IDisposable? _disposable;

    public CanvasItem()
    {
        InitializeComponent();

        var border = Border1;

        PointerPressed += (_, _) =>
        {
            border.Background = Brushes.Green;
            _disposable?.Dispose();
        };

        _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => Move());
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
