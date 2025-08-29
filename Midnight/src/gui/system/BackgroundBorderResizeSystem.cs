namespace Midnight.GUI;

[SystemRegistry]
public sealed class BackgroundBorderResizeSystem : EntitySystem {
    public override void Setup(Scene scene) {
        Subscribe<ResizeEvent>()
            .With<BackgroundBorder>()
            .Submit(Resize);
    }

    public void Resize(ResizeEvent e, Query<BackgroundBorder> backgroundBorder) {
        //Logger.DebugLine($"{GetType()} -> Resize at {backgroundBorder.Entry.Entity}");
        if (backgroundBorder.Entry.Background != null) {
            //Logger.DebugLine("Applying to Background");
            ResizeDrawable(backgroundBorder.Entry.Background, e.Size);
        }

        if (backgroundBorder.Entry.Border != null) {
            //Logger.DebugLine("Applying to Border");
            ResizeDrawable(backgroundBorder.Entry.Border, e.Size);
        }
    }

    private void ResizeDrawable(Drawable drawable, Size2 size) {
        drawable.Size = size;
    }
}
