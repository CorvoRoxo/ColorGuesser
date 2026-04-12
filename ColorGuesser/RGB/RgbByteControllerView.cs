namespace ColorGuesser;

public partial class RgbByteControllerView : ContentView
{
    public Grid MainGrid { get; set; } = new();
    public Label ValueName { get; set; } = new();
    public Editor ValueEditor { get; set; } = new();
    public Slider ValueSlider { get; set; } = new();
    private ColorRGB? RgbToControl { get; set; }
    
    private static readonly BindableProperty ValueTypeProperty = BindableProperty.Create(nameof(ValueType), typeof(RgbByte), typeof(RgbByteControllerView), RgbByte.R);
    public RgbByte ValueType
    {
        get => (RgbByte)GetValue(ValueTypeProperty);
        set => SetValue(ValueTypeProperty, value);
    }

    public RgbByteControllerView(RgbByte ValueType, ColorRGB RgbToControl)
    {
        this.RgbToControl = RgbToControl;
        this.ValueType = ValueType;

        ValueName.Text = $"{this.ValueType}";
        ValueName.VerticalTextAlignment = TextAlignment.Center;
        
        ValueSlider.Maximum = 255;
        ValueSlider.MinimumTrackColor = GetColorByRgbByte(ValueType);
        
        ValueEditor.SetBinding(Editor.TextProperty, new Binding(source: ValueSlider, path: "Value", mode: BindingMode.TwoWay, stringFormat: "{0:F0}"));

        ValueSlider.ValueChanged += (s, e) =>
        {
            int Value = (int)ValueSlider.Value;
            switch (this.ValueType)
            {
                case RgbByte.R:
                    RgbToControl.R_Value = Value;
                    break;
                
                case RgbByte.G:
                    RgbToControl.G_Value = Value;
                    break;
                
                case RgbByte.B:
                    RgbToControl.B_Value = Value;
                    break;
            }

            RgbToControl.SetCompleteColor();
        };
        
        MainGrid.ColumnDefinitions = new()
        {
            new(GridLength.Auto),
            new(GridLength.Auto),
            new(GridLength.Star)
        };
        MainGrid.Add(ValueName, 0);
        MainGrid.Add(ValueEditor, 1);
        MainGrid.Add(ValueSlider, 2);
        
        this.Content = MainGrid;
    }

    private Color GetColorByRgbByte(RgbByte RgbByte)
    {
        switch (RgbByte)
        {
            case RgbByte.R:
                return new Color(255, 0, 0);
            
            case RgbByte.G:
                return new Color(0, 255, 0);
            
            case RgbByte.B:
                return new Color(0, 0, 255);
            
            default:
                return new Color(0, 0, 0);
        }
    }
}