using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.Assets.Aseprite;

public class AsepriteFrame {
    private Color[] _buffer;
    private Texture2D _texture;

    internal AsepriteFrame(AsepriteData data) {
        Data = data;
    }

    public AsepriteData Data { get; }
    public List<(AsepriteLayer Layer, AsepriteCel Cel)> Cels { get; } = new();

    public AsepriteCel GetCel(int layer) {
        foreach ((AsepriteLayer Layer, AsepriteCel Cel) entry in Cels) {
            if (entry.Layer.Id == layer) {
                return entry.Cel;
            }
        }

        return null;
    }

    public bool HasCel(int layer) {
        foreach ((AsepriteLayer Layer, AsepriteCel Cel) entry in Cels) {
            if (entry.Layer.Id == layer) {
                return true;
            }
        }

        return false;
    }

    public Texture2D Extract() {
        // prepare texture
        if (_texture == null) {
            _texture = new(Data.Size.Width, Data.Size.Height);
        } else if (_texture.Size != Data.Size) {
            _texture.Release();
            _texture = new(Data.Size.Width, Data.Size.Height);
        }

        // prepare color buffer
        if (_buffer == null) {
            _buffer = new Color[Data.Size.Area];
        } else if (_buffer.Length < Data.Size.Area) {
            System.Array.Resize(ref _buffer, Data.Size.Area);
        }

        // paint each cel at this frame, layer by layer
        foreach ((AsepriteLayer Layer, AsepriteCel Cel) entry in Cels) {
            switch (entry.Cel) {
                case AsepriteCompressedImageCel imageCel:
                    Paint(imageCel, imageCel.Texture, entry.Layer);
                    break;
                case AsepriteLinkedCel linkedCel:
                    {
                        if (linkedCel.GetLinked() is AsepriteCompressedImageCel imageCel) {
                            Paint(linkedCel, imageCel.Texture, entry.Layer);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        _texture.Write(_buffer, 0, Data.Size.Area);
        return _texture;
    }

    private void Paint(AsepriteCel cel, Texture2D srcTexture, AsepriteLayer srcLayer) {
        // prepare src buffer
        Color[] src = new Color[srcTexture.Size.Area];
        srcTexture.Read(src);

        // get layer blend function
        System.Func<Color, Color, byte, Color> blendFunc;
        switch (srcLayer.BlendMode) {
            case AsepriteBlendMode.Normal:
                blendFunc = BlendNormal;
                break;
            default:
                throw new System.NotImplementedException("Blend function: " + srcLayer.BlendMode);
        }

        // opacity (with cel and layer opacity)
        byte opacity = (byte) Math.Clamp(srcLayer.Opacity * (cel.Opacity / 255.0f), 0, 255);

        // apply blend function at target rect
        for (int y = 0; y < srcTexture.Size.Height; y++) {
            for (int x = 0; x < srcTexture.Size.Width; x++) {
                int targetX = cel.Position.X + x,
                    targetY = cel.Position.Y + y,
                    targetIndex = targetX + targetY * Data.Size.Width,
                    sourceIndex = x + y * srcTexture.Size.Width;

                _buffer[targetIndex] = blendFunc(
                    _buffer[targetIndex],
                    src[sourceIndex],
                    opacity
                );
            }
        }
    }

    private Color BlendNormal(Color back, Color source, byte opacity) {
        byte sourceA = (byte) Math.Clamp(source.A * opacity / 255.0f, 0, 255);

        if (back.A <= 1) {
            return source.WithA(sourceA);
        } else if (source.A <= 1) {
            return back;
        }

        byte a = (byte) Math.Clamp(sourceA + back.A - back.A * (sourceA / 255.0f), 0, 255);
        byte r = (byte) Math.Clamp(source.R + (source.R - back.R) * sourceA / (float) a, 0, 255);
        byte g = (byte) Math.Clamp(source.G + (source.G - back.G) * sourceA / (float) a, 0, 255);
        byte b = (byte) Math.Clamp(source.B + (source.B - back.B) * sourceA / (float) a, 0, 255);
        return new(r, g, b, a);
    }
}
