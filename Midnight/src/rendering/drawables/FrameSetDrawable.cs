using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public class FrameSetDrawable : RectangleDrawable {
    private List<RectangleI> _frames = new();
    private int _frameIndex = -1;

    public FrameSetDrawable() {
    }

    public int FrameIndex {
        get => _frameIndex;
        set {
            Assert.InRange(value, 0, _frames.Count - 1);
            _frameIndex = value;
            ClipRegion = _frames[_frameIndex];
            RequestRecalculateVertices();
        }
    }

    public Vector2I FrameCell {
        get {
            Assert.NotNull(Texture);
            Assert.True(DefaultFrameSize.HasValue);
            int columns = Texture.Size.Width / DefaultFrameSize.Value.Width;
            return new(_frameIndex % columns, _frameIndex / columns);
        }
        set {
            Assert.NotNull(Texture);
            Assert.True(DefaultFrameSize.HasValue);
            int columns = Texture.Size.Width / DefaultFrameSize.Value.Width;
            FrameIndex = value.X + value.Y * columns;
        }
    }

    public Size2I? DefaultFrameSize { get; set; }

    public void AutoRegisterGrid(Size2I frameSize) {
        Assert.NotNull(Texture);
        DefaultFrameSize = frameSize;
        Size2I amount = Texture.Size / frameSize;

        for (int y = 0; y < amount.Height; y++) {
            for (int x = 0; x < amount.Width; x++) {
                RegisterFrame(new(
                    new Vector2I(x, y) * frameSize,
                    frameSize
                ));
            }
        }
    }

    public void RegisterFrame(RectangleI bounds) {
        _frames.Add(bounds);

        if (FrameIndex < 0) {
            FrameIndex = 0;
        }
    }

    public void RegisterFrameAt(Vector2I position) {
        Assert.True(DefaultFrameSize.HasValue);
        RegisterFrame(new(position, DefaultFrameSize.Value));
    }

    public void Clear() {
        _frames.Clear();
    }
}
