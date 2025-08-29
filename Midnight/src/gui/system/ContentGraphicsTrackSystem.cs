namespace Midnight.GUI;

[SystemRegistry]
public sealed class ContentGraphicsTrackSystem : EntitySystem {
    public override void Setup(Scene scene) {
        Subscribe<ECS.ComponentAddedEvent>()
            .With<ContentGraphics>()
            .Submit(ComponentAdded)
            .HandleOnce();
    }

    public void ComponentAdded(ECS.ComponentAddedEvent e, Query<ContentGraphics> contentGraphics) {
        //Logger.DebugLine($"{GetType()} -> ComponentAdded '{e.Context.GetType()}' to {contentGraphics.Entry.Entity}");

        switch (e.Context) {
            case Drawable drawable:
                {
                    if (contentGraphics.Entry.Entity.TryGet<BackgroundBorder>(out BackgroundBorder backgroundBorder)) {
                        Upkeep(contentGraphics.Entry, backgroundBorder);

                        if (backgroundBorder.Background == drawable || backgroundBorder.Border == drawable) {
                            // ignore background or border
                            //Logger.DebugLine($"Ignore background or border");
                            return;
                        }
                    }

                    if (!contentGraphics.Entry.Entries.Contains(drawable)) {
                        contentGraphics.Entry.Entries.Add(drawable);
                        //Logger.DebugLine($"Registered '{e.Context.GetType()}' to ContentGraphics (total: {contentGraphics.Entry.Entries.Count})");
                    } else {
                        //Logger.DebugLine($"Already registered to ContentGraphics (total: {contentGraphics.Entry.Entries.Count})");
                    }
                }
                break;
            case BackgroundBorder backgroundBorder:
                Upkeep(contentGraphics.Entry, backgroundBorder);
                break;
            default:
                break;
        }
    }

    private void Upkeep(ContentGraphics contentGraphics, BackgroundBorder backgroundBorder) {
        if (backgroundBorder.Background != null) {
            contentGraphics.Entries.Remove(backgroundBorder.Background);
        }

        if (backgroundBorder.Border != null) {
            contentGraphics.Entries.Remove(backgroundBorder.Border);
        }
    }
}
