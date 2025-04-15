
namespace Midnight;

public class GridDrawable : Drawable {
    public GridDrawable() {
        Tile = new RectangleDrawable() {
            Color = 0x00000020,
            Filled = true,
            Size = new(32, 32),
        };
    }

    public Drawable Tile { get; set; }
    public Size2 TileSize => Tile?.Size ?? Size2.Zero;

    public override Size2 Size {
        get => RenderingServer.MainCamera.Size.ToFloat();
        set {
            throw new System.NotSupportedException();
        }
    }

    public void SetupTile(Size2 size, Color color) {
        RectangleDrawable tile;

        if (Tile is RectangleDrawable t) {
            tile = t;
        } else {
            Tile = tile = new RectangleDrawable();
        }

        tile.Color = color;
        tile.Size = size;
    }

    protected override void Paint(DeltaTime dt) {
        if (Tile == null) {
            return;
        }

        Camera camera = RenderingServer.MainCamera;

        int columns = Math.CeilI(camera.Size.Width / TileSize.Width),
            rows = Math.CeilI(camera.Size.Height / TileSize.Height),
            startX = 0;

        DrawSettings settings = Midnight.DrawSettings.Default with {
            WorldViewProjection = camera.Projection,
        };

        // using camera to positioning
        Vector2 displacement = -(camera.Position.ToVec2() % TileSize);

        // calculate correction if tile is odd or even
        Vector2 tile = (camera.Position.ToVec2() / TileSize).Round(System.MidpointRounding.ToZero);
        displacement += (tile % new Vector2(2, 2)) * TileSize;

        for (int y = -1; y < rows + 1; y++) {
            for (int x = startX - 1; x < columns + 1; x += 2) {
                Tile.Transform.Position = displacement + new Vector2(x, y) * Tile.Size;
                Tile.Draw(
                    dt,
                    new DrawParams { DrawSettings = settings }
                );

            }

            startX = (startX + 1) % 2;
        }
    }

    protected override void UpdateVertices() {
        // nothing to do
    }
}
