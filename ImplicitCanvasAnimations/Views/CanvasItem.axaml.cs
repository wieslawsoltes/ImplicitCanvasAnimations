using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ImplicitCanvasAnimations.Views;

public partial class CanvasItem : UserControl
{
    public CanvasItem()
    {
        InitializeComponent();

        PointerPressed += (_, _) =>
        {
            if (Parent is Canvas canvas)
            {
                var left = Random.Shared.NextDouble() * canvas.Bounds.Width;
                var top = Random.Shared.NextDouble() * canvas.Bounds.Height;

                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
            }
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
