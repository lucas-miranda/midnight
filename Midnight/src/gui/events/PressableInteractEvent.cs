
namespace Midnight.GUI;

public class PressableInteractEvent : UIEvent, IEventOriginator<Entity> {
    public PressableInteractEvent(Pressable pressable) {
        Pressable = pressable;
    }

    public Entity Originator => Pressable.Entity;
    public Pressable Pressable { get; }
}
