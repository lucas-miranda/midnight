using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class RenderLayers : IEnumerable<RenderLayer> {
    private List<RenderLayer> _layers = new();
    private SpriteDisplayer _displayer;

    internal RenderLayers() {
    }

    public int Count => _layers.Count;

    public void Register(int order, Canvas canvas) {
        Assert.NotNull(canvas);
        _layers.Add(new(canvas, order));
        _layers.Sort(Sort);
    }

    public bool Remove(Canvas canvas) {
        int index = IndexOf(canvas);

        if (index < 0) {
            return false;
        }

        _layers.RemoveAt(index);
        return true;
    }

    public int IndexOf(Canvas canvas) {
        int i = 0;
        foreach (RenderLayer layer in _layers) {
            if (layer.Canvas == canvas) {
                return i;
            }

            i += 1;
        }

        return -1;
    }

    public void Clear() {
        _layers.Clear();
    }

    public IEnumerator<RenderLayer> GetEnumerator() {
        return _layers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private int Sort(RenderLayer a, RenderLayer b) {
        return a.Order.CompareTo(b.Order);
    }

    internal void LoadContent() {
        _displayer = new() {
            Texture = null,
            DrawSettings = SpriteDisplayer.DefaultDrawSettings with {
                Immediate = true,
            },
        };
    }

    internal void ResourceRelease() {
    }

    internal void Render(DeltaTime dt) {
        // ensure we don't use camera wvp, we only need it's projection
        _displayer.DrawSettings = _displayer.DrawSettings with {
            WorldViewProjection = RenderingServer.MainCamera.Projection
        };

        foreach (RenderLayer layer in _layers) {
            _displayer.Texture = layer.Canvas;
            _displayer.Render(dt);
        }

        _displayer.Texture = null;
    }
}

public record class RenderLayer(Canvas Canvas, int Order);
