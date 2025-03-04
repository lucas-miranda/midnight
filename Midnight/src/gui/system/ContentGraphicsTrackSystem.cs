namespace Midnight.GUI;

public sealed class ContentGraphicsTrackSystem : EntitySystem {
    public override void Setup() {
        Subscribe<ECS.ComponentAddedEvent, ContentGraphics>(ComponentAdded, matchOriginatorOnly: true);
    }

    public void ComponentAdded(ECS.ComponentAddedEvent e, ContentGraphics contentGraphics) {
        Logger.DebugLine($"{GetType()} -> ComponentAdded '{e.Context.GetType()}' to {contentGraphics.Entity}");

        switch (e.Context) {
            case GraphicDisplayer graphicDisplayer:
                {
                    if (contentGraphics.Entity.TryGet<BackgroundBorder>(out BackgroundBorder backgroundBorder)) {
                        Upkeep(contentGraphics, backgroundBorder);

                        if (backgroundBorder.Background == graphicDisplayer || backgroundBorder.Border == graphicDisplayer) {
                            // ignore background or border
                            Logger.DebugLine($"Ignore background or border");
                            return;
                        }
                    }

                    if (!contentGraphics.Entries.Contains(graphicDisplayer)) {
                        contentGraphics.Entries.Add(graphicDisplayer);
                        Logger.DebugLine($"Registered '{e.Context.GetType()}' to ContentGraphics (total: {contentGraphics.Entries.Count})");
                    } else {
                        Logger.DebugLine($"Already registered to ContentGraphics (total: {contentGraphics.Entries.Count})");
                    }
                }
                break;
            case BackgroundBorder backgroundBorder:
                Upkeep(contentGraphics, backgroundBorder);
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
