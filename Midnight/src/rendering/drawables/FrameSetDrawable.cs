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

    public void AutoRegisterGrid(Size2I frameSize, Vector2I? startPos = null) {
        Assert.NotNull(Texture);
        DefaultFrameSize = frameSize;
        Vector2I pos = startPos.GetValueOrDefault();
        Size2I amount = (Texture.Size - new Size2I(pos)) / frameSize;

        for (int y = 0; y < amount.Height; y++) {
            for (int x = 0; x < amount.Width; x++) {
                RegisterFrame(new(
                    pos + new Vector2I(x, y) * frameSize,
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

        if (!DefaultFrameSize.HasValue) {
            DefaultFrameSize = bounds.Size;
        }
    }

    public void RegisterFrameAt(Vector2I position) {
        Assert.True(DefaultFrameSize.HasValue);
        RegisterFrame(new(position, DefaultFrameSize.Value));
    }

    public void RegisterFrames(RectangleI bounds, Size2I frameSize) {
        int columns = bounds.Width / frameSize.Width,
            rows = bounds.Height / frameSize.Height;

        Vector2I pos = bounds.TopLeft;

        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < columns; x++) {
                RegisterFrame(new(
                    bounds.TopLeft + new Vector2I(x, y) * frameSize,
                    frameSize
                ));
            }
        }
    }

    public void RegisterFrames(Vector2I startPos, Size2I frameSize, Size2I cells) {
        RegisterFrames(new(startPos, frameSize * cells), frameSize);
    }

    public void Clear() {
        _frames.Clear();
    }

    public void Centralize() {
        Assert.True(_frameIndex >= 0);
        RectangleI frame = _frames[FrameIndex];
        Transform.Position = -(frame.Size / 2).ToVector2();
    }
}
