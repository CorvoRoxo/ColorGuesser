namespace ColorGuesser;

public partial class RgbColorControllerView : ContentView
{
    public VerticalStackLayout MainVerticalLayout { get; set; } = new();
    public BoxView ColorDisplay { get; set; } = new();
    private ColorRGB _ColorRgb = new();

    public RgbByteControllerView R_Controller { get; set; }
    public RgbByteControllerView G_Controller { get; set; }
    public RgbByteControllerView B_Controller { get; set; }

    private static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(RgbColorControllerView));

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    
    private static readonly BindableProperty VisibleColorDisplayProperty = BindableProperty.Create(nameof(VisibleColorDisplay), typeof(bool), typeof(RgbColorControllerView), true);
    public bool VisibleColorDisplay
    {
        get => (bool)GetValue(VisibleColorDisplayProperty);
        set => SetValue(VisibleColorDisplayProperty, value);
    }

    public RgbColorControllerView()
    {
        this.R_Controller = new(RgbByte.R, _ColorRgb);
        this.R_Controller.ValueType = RgbByte.R;

        this.G_Controller = new(RgbByte.G, _ColorRgb);
        this.G_Controller.ValueType = RgbByte.G;

        this.B_Controller = new(RgbByte.B, _ColorRgb);
        this.B_Controller.ValueType = RgbByte.B;

        ColorDisplay.HeightRequest = 45;
        ColorDisplay.BindingContext = this;
        ColorDisplay.SetBinding(BackgroundColorProperty, new Binding(source: _ColorRgb, path: "CompleteColor"));
        ColorDisplay.SetBinding(BoxView.IsVisibleProperty, "VisibleColorDisplay");
        
        this.SetBinding(ColorProperty, new Binding(source: _ColorRgb, path: "CompleteColor"));

        MainVerticalLayout.Add(ColorDisplay);
        MainVerticalLayout.Add(R_Controller);
        MainVerticalLayout.Add(G_Controller);
        MainVerticalLayout.Add(B_Controller);

        this.Content = MainVerticalLayout;
    }
}