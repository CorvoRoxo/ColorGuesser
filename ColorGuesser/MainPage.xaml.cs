using System.Text;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorGuesser;

public partial class MainPage : ContentPage
{
    public static Random Random = new();
    
    ColorGuesserVM ColorGuesserVM { get; set; } = new();
    private Color UserColor { get; set; } = new(0, 0, 0);

    public MainPage()
    {
        InitializeComponent();
        
        ColorGuesserVM.SetNewHexColor();
        this.BindingContext = ColorGuesserVM;
        
    }
    
    private void GuessColorButton_OnClicked(object? sender, EventArgs e)
    {
        UserColor = UserColorInput.Color;
        UserColorToHex_Label.Text = $"{UserColor.ToHex()}";
        
        ColorGuesserVM.IsColorWritedCorrect = UserColor.ToHex().ToLower() == ColorGuesserVM.PickedHexColor.ToLower();
        System.Diagnostics.Debug.WriteLine($"{UserColor.ToHex()} == {ColorGuesserVM.PickedHexColor}");
        
        if (!ColorGuesserVM.IsColorWritedCorrect)
            return;
        
        ColorGuesserVM.SetNewHexColor();
    }

    private void HideHexColorLabelButton_OnClicked(object? sender, EventArgs e)
    {
        PickedColor.IsVisible = !PickedColor.IsVisible;
    }

    private void RGBColor_Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        
    }
}

public partial class ColorGuesserVM : ObservableObject
{
    private static Random Random = new();
    
    [ObservableProperty]
    private string _PickedHexColor = "#FF5600";

    [ObservableProperty]
    private string _PickedHexColorCodeOnly = "FF5600";

    [ObservableProperty]
    private string _WritedHexColor = "#";

    [ObservableProperty]
    private bool _IsColorWritedCorrect;

    [ObservableProperty]
    private ProximityLevel _ProximityLevel;
    
    public static Dictionary<char, int> HexChars = new()
    {
        {'a', 10}, {'b', 11}, {'c', 12}, {'d', 13}, {'e', 14}, {'f', 15},
        {'A', 10}, {'B', 11}, {'C', 12}, {'D', 13}, {'E', 14}, {'F', 15}
    };

    public void SetNewHexColor()
    {
        string NewPickedHexColor = "#";
        
        for (int i = 1; i <= 6; i++)
            NewPickedHexColor += (Random.Next(2) == 1)
                ? Random.Next(10).ToString()
                : HexChars.ElementAt(Random.Next(HexChars.Count())).Key;

        this.PickedHexColor = NewPickedHexColor;
        this.PickedHexColorCodeOnly = NewPickedHexColor.Substring(0, 1);
    }
}

public enum ProximityLevel
{
    Near,
    Correct,
    Distant
}