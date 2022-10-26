using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ImplicitCanvasAnimations.Views;

public partial class CanvasItem : UserControl
{
    public CanvasItem()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}