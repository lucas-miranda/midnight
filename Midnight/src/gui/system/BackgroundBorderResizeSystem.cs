namespace Midnight.GUI;

public sealed class BackgroundBorderResizeSystem : EntitySystem {
    public override void Setup() {
        Subscribe<ResizeEvent, BackgroundBorder>(Resize);
    }

    public void Resize(ResizeEvent e, BackgroundBorder backgroundBorder) {
        Logger.DebugLine($"{GetType()} -> Resize at {backgroundBorder.Entity}");
        if (backgroundBorder.Background != null) {
            Logger.DebugLine("Applying to Background");
            ResizeDrawable(backgroundBorder.Background.Drawable, e.Size);
        }

        if (backgroundBorder.Border != null) {
            Logger.DebugLine("Applying to Border");
            ResizeDrawable(backgroundBorder.Border.Drawable, e.Size);
        }
    }

    private void ResizeDrawable(Drawable drawable, Size2 size) {
        drawable.Size = size;
    }
}
