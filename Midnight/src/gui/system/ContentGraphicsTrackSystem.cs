namespace Midnight.GUI;

[SystemRegistry]
public sealed class ContentGraphicsTrackSystem : EntitySystem {
    public override void Setup(Scene scene) {
        var contract = Subscribe<ECS.ComponentAddedEvent>()
            .With<ContentGraphics>()
            .Submit(ComponentAdded);

        contract.MatchOriginatorOnly = true;
    }

    public void ComponentAdded(ECS.ComponentAddedEvent e, Query<ContentGraphics> contentGraphics) {
        Logger.DebugLine($"{GetType()} -> ComponentAdded '{e.Context.GetType()}' to {contentGraphics.Entry.Entity}");

        switch (e.Context) {
            case GraphicDisplayer graphicDisplayer:
                {
                    if (contentGraphics.Entry.Entity.TryGet<BackgroundBorder>(out BackgroundBorder backgroundBorder)) {
                        Upkeep(contentGraphics.Entry, backgroundBorder);

                        if (backgroundBorder.Background == graphicDisplayer || backgroundBorder.Border == graphicDisplayer) {
                            // ignore background or border
                            Logger.DebugLine($"Ignore background or border");
                            return;
                        }
                    }

                    if (!contentGraphics.Entry.Entries.Contains(graphicDisplayer)) {
                        contentGraphics.Entry.Entries.Add(graphicDisplayer);
                        Logger.DebugLine($"Registered '{e.Context.GetType()}' to ContentGraphics (total: {contentGraphics.Entry.Entries.Count})");
                    } else {
                        Logger.DebugLine($"Already registered to ContentGraphics (total: {contentGraphics.Entry.Entries.Count})");
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
