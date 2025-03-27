using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.Assets.Aseprite;

public enum AsepriteColorDepth {
    Undefined   = 0,
    Indexed     = 8,
    Grayscale   = 16,
    RGBA        = 32,
}

public enum AsepriteFlags {
    None = 0,
    LayerOpacityValidValue = 1 << 0,
}

[System.Flags]
public enum AsepriteLayerFlags {
    None             = 0,
    Visible          = 1 << 0,
    Editable         = 1 << 1,
    LockMovement     = 1 << 2,
    Background       = 1 << 3,
    PreferLinkedCels = 1 << 4,
    Collapsed        = 1 << 5,
    Reference        = 1 << 6,
}

[System.Flags]
public enum AsepriteCelFlags {
    None = 0,
    PreciseBounds = 1 << 0,
}

public enum AsepriteBlendMode {
    Normal         = 0,
    Multiply       = 1,
    Screen         = 2,
    Overlay        = 3,
    Darken         = 4,
    Lighten        = 5,
    ColorDodge     = 6,
    ColorBurn      = 7,
    HardLight      = 8,
    SoftLight      = 9,
    Difference     = 10,
    Exclusion      = 11,
    Hue            = 12,
    Saturation     = 13,
    Color          = 14,
    Luminosity     = 15,
    Addition       = 16,
    Subtract       = 17,
    Divide         = 18,
}

public class AsepriteData {
    public Size2I Size { get; set; }
    public AsepriteColorDepth ColorDepth { get; set; }
    public AsepriteFlags Flags { get; set; }
    public int TransparentPaletteIndex { get; set; }
    public Size2I PixelSize { get; set; }
    public Vector2I GridPosition { get; set; }
    public Size2I GridSize { get; set; }
    public AsepritePalette Palette { get; set; }
    public List<AsepriteLayer> Layers { get; } = new();
    public List<AsepriteFrame> Frames { get; } = new();

    public void Place(int layer, AsepriteCel cel) {
        Assert.False(layer < 0 || layer >= Layers.Count);
        AsepriteFrame frame;
        int frameIndex;

        if (Frames.Count > 0) {
            frameIndex = Frames.Count - 1;
            AsepriteFrame lastFrame = Frames[frameIndex];

            // check if we should place cel at last frame or we'll need a new one
            bool needNewFrame = lastFrame.Cels.Count == Layers.Count
                || (lastFrame.Cels.Count > 0 && lastFrame.Cels[^1].Layer.Id >= layer);

            if (needNewFrame) {
                // we need a new frame
                frame = new(this);
                Frames.Add(frame);
                frameIndex = Frames.Count - 1;
            } else {
                frame = lastFrame;
            }
        } else {
            // we need a new frame
            frame = new(this);
            Frames.Add(frame);
            frameIndex = Frames.Count - 1;
        }

        cel.Layer = Layers[layer];
        Logger.Line($"To Layer: {layer}");
        cel.Layer.Add(frameIndex, cel);
        frame.Cels.Add((cel.Layer, cel));
    }

    public string TimelineToString() {
        string s = "";

        for (int i = 0; i < Frames.Count; i++) {
            s += $"#{Frames[i].Cels.Count} |";
        }

        s += "\n";

        for (int i = 0; i < Frames.Count; i++) {
            s += $" {i} |";
        }

        s += "\n";
        s += new string('-', 4 * Frames.Count);
        s += "\n";

        for (int k = Layers.Count - 1; k >= 0; k--) {
            for (int i = 0; i < Frames.Count; i++) {
                AsepriteCel cel = Frames[i].GetCel(k);

                switch (cel) {
                    case AsepriteLinkedCel linkedCel:
                        if (linkedCel.FromFrame < i) {
                            s += $"===O";
                        } else {
                            s += $" O";
                        }
                        break;
                    case null:
                        if (i > 0) {
                            s += $" | x";
                        } else {
                            s += $" x";
                        }
                        break;
                    default:
                        if (i > 0) {
                            s += $" | O";
                        } else {
                            s += $" O";
                        }
                        break;
                }
            }

            s += " |\n";
        }

        return s;
    }
}
