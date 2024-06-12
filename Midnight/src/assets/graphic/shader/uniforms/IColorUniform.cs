namespace Midnight;

public interface IColorUniform {
    ColorF ColorF { get; set; }
    Color Color { get => ColorF.ToByte(); set => ColorF = value.Normalized(); }
}
