using System.Diagnostics;
using System.Text;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorGuesser;

public partial class MainPage : ContentPage
{
    private RandomColorPicker RandomColorPicker { get; set; } = new();
    private Color UserColor { get; set; } = new(0, 0, 0);

    public MainPage()
    {
        this.BindingContext = RandomColorPicker;
        InitializeComponent();
        
        PickNewRandomColorWitHAnimation();
        PickedColorDisplayLabel.SetBinding(Label.TextProperty, new Binding {Source = PickedColorDisplay, Path = "BackgroundColor", Converter = new ColorToHexRgbStringConverter()});
    }
    
    private void GuessColorButton_OnClicked(object? sender, EventArgs e)
    {
        UserColor = UserColorInput.Color;

        bool IsUserColorInputEquals = UserColor.ToHex() == RandomColorPicker.ColorPicked.ToHex();
        Console.WriteLine($"{UserColor.ToHex()} == {RandomColorPicker.ColorPicked.ToHex()}: {IsUserColorInputEquals}");
        
        if (!IsUserColorInputEquals)
            return;

        PickNewRandomColorWitHAnimation();

        SetRgbSliderNewValue(UserColorInput.R_Controller.ValueSlider,0, Easing: Easing.BounceOut);
        SetRgbSliderNewValue(UserColorInput.G_Controller.ValueSlider,0, Easing: Easing.BounceOut);
        SetRgbSliderNewValue(UserColorInput.B_Controller.ValueSlider,0, Easing: Easing.BounceOut);
        Console.WriteLine($"New Color Selected: {RandomColorPicker.ColorPicked.ToHex()}");
    }

    private void PickNewRandomColor_ButtonOnClick(object? sender, EventArgs e)
    {
        PickNewRandomColorWitHAnimation();
    }

    private void PickNewRandomColorWitHAnimation()
    {
        RandomColorPicker.PickNewColor();
        Color CurrentColor = PickedColorDisplay.BackgroundColor;
        PickedColorDisplay.BackgroundColorTo(RandomColorPicker.ColorPicked, 16, 1000, Easing.CubicInOut);
    }

    private void SetRgbSliderNewValue(Slider Slider, double NewValue, uint Rate = 16U, uint Length = 1000, Easing? Easing = null)
    {
        double SliderCurrentValue = Slider.Value;
        Animation SliderAnimation = new(V => Slider.Value = V, SliderCurrentValue, NewValue);
        SliderAnimation.Commit(this, $"{Slider.Id}SliderValueAnimation", Rate, Length, Easing);
    }
}

public partial class RandomColorPicker : ObservableObject
{
    private static Random Random = new();
    
    [ObservableProperty] 
    private Color _ColorPicked = new(0, 0, 0);

    public void PickNewColor()
    {
        byte Red = (byte)Random.Next(255);
        byte Green = (byte)Random.Next(255);
        byte Blue = (byte)Random.Next(255);
        ColorPicked = new Color(Red, Green, Blue);
    }
}