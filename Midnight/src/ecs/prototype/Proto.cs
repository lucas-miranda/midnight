
namespace Midnight;

public static class Proto {
    public static Entity New<T>(Entity? parent = null) {
        return Prototypes.Instantiate<T>(parent);
    }
}
