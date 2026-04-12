using System;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorGuesser;

public partial class MainPage : ContentPage
{
    private static Random Random = new();
    
    private RandomColorPicker RandomColorPicker { get; set; } = new();
    private Color UserColor { get; set; } = new(0, 0, 0);

    public MainPage()
    {
        this.BindingContext = RandomColorPicker;
        InitializeComponent();
        
        PickNewRandomColorWithAnimation();
        PickedColorDisplayLabel.SetBinding(Label.TextProperty, new Binding {Source = PickedColorDisplay, Path = "BackgroundColor", Converter = new ColorToHexRgbStringConverter()});
    }
    
    private void GuessColorButton_OnClicked(object? sender, EventArgs e)
    {
        UserColor = UserColorInput.Color;

        bool IsUserColorInputEquals = UserColor.ToHex() == RandomColorPicker.ColorPicked.ToHex();
        Console.WriteLine($"{UserColor.ToHex()} == {RandomColorPicker.ColorPicked.ToHex()}: {IsUserColorInputEquals}");
        Console.WriteLine($"'{UserColor}' == '{RandomColorPicker.ColorPicked}'");
        Console.WriteLine($"IsColorsNear: {IsColorsNear(UserColor, RandomColorPicker.ColorPicked)}");

        Task.Run(async () =>
        {
            await GuessColorButtonMainGrid.ScaleToAsync(1.05, 125, Easing.SinOut);
            await GuessColorButtonMainGrid.ScaleToAsync(1, 85, Easing.SinOut);
        });
        
        if (!IsUserColorInputEquals)
        {
            ApplyIncorrectColorInputAnimation();
            return;
        }

        PickNewRandomColorWithAnimation();
        ResetUserInputColorWithAnimation();
        
        Console.WriteLine($"New Color Selected: {RandomColorPicker.ColorPicked.ToHex()}");
    }

    public bool IsColorsNear(Color ColorA, Color ColorB)
    {
        bool IsRedNear = ColorA.Red * 255 < ColorB.Red * 255 + 55 && ColorA.Red * 255 > ColorB.Red * 255 - 55;
        bool IsGreenNear = ColorA.Green * 255 < ColorB.Green * 255 + 55 && ColorA.Green * 255 > ColorB.Green * 255 - 55;
        bool IsBlueNear = ColorA.Blue * 255 < ColorB.Blue * 255 + 55 && ColorA.Blue * 255 > ColorB.Blue * 255 - 55;

        int NearCount = 0;

        if (IsRedNear)
            NearCount++;

        if (IsGreenNear)
            NearCount++;

        if (IsBlueNear)
            NearCount++;
        
        Console.WriteLine($"IsRedNear: {IsRedNear}  |  IsGreenNear: {IsGreenNear}  |  IsBlueNear: {IsBlueNear}");
        return NearCount >= 2;
    }

    private void PickNewRandomColorWithAnimation_ButtonOnClick(object? sender, EventArgs e)
        => PickNewRandomColorWithAnimation();
    
    private void ResetUserInputColorWithAnimation_ButtonOnClick(object? sender, EventArgs e)
        => ResetUserInputColorWithAnimation();

    private void PickNewRandomColorWithAnimation()
    {
        RandomColorPicker.PickNewColor();
        Color CurrentColor = PickedColorDisplay.BackgroundColor;
        PickedColorDisplay.BackgroundColorTo(RandomColorPicker.ColorPicked, 16, 1000, Easing.CubicInOut);
    }

    public void ResetUserInputColorWithAnimation()
    {
        SetRgbSliderNewValue(UserColorInput.R_Controller.ValueSlider,0, Length: (uint)Random.Next(800, 1400), Easing: Easing.BounceOut);
        SetRgbSliderNewValue(UserColorInput.G_Controller.ValueSlider,0, Length: (uint)Random.Next(800, 1400), Easing: Easing.BounceOut);
        SetRgbSliderNewValue(UserColorInput.B_Controller.ValueSlider,0, Length: (uint)Random.Next(800, 1400), Easing: Easing.BounceOut);
    }

    private void SetRgbSliderNewValue(Slider Slider, double NewValue, uint Rate = 16U, uint Length = 1000, Easing? Easing = null)
    {
        if (NewValue == Slider.Value)
            return;
        
        double SliderCurrentValue = Slider.Value;
        Animation SliderAnimation = new(V => Slider.Value = V, SliderCurrentValue, NewValue);
        SliderAnimation.Commit(this, $"{Slider.Id}SliderValueAnimation", Rate, Length, Easing);
    }

    private async void ApplyIncorrectColorInputAnimation()
    {
        for (int i = 1; i <= 4; i++)
        {
            await GuessColorButtonMainGrid.RotateToAsync(5, 45, Easing.SinOut);
            await GuessColorButtonMainGrid.RotateToAsync(-5, 45, Easing.SinOut);
        }
        await GuessColorButtonMainGrid.RotateToAsync(0, 125, Easing.SinOut);
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