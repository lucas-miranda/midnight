using System.Collections;

namespace Midnight;

public static class ICollectionExtensions {
    public static bool IsEmpty(this ICollection c) {
        return c.Count <= 0;
    }
}
