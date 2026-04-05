using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm;
using Microsoft.Maui.Graphics;

namespace ColorGuesser;

public partial class ColorRGB : ObservableObject
{
    [ObservableProperty]
    private int _R_Value = 0;
    
    [ObservableProperty]
    private int _G_Value = 0;

    [ObservableProperty]
    private int _B_Value = 0;
    
    [ObservableProperty]
    private Color _CompleteColor = new(0, 0, 0);

    public Color SetCompleteColor()
        => CompleteColor = new(R_Value, G_Value, B_Value);
}