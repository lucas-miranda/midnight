using System.Collections;
using System.Collections.Generic;

namespace Midnight;

public static class ICollectionExtensions {
    public static bool IsEmpty(this ICollection c) {
        return c.Count <= 0;
    }

    /// <summary>Add from <paramref name="range"/> but casts every item from <typeparamref name="U"/> to <typeparamref name="T"/>.</summary>
    public static void AddRange<T, U>(this ICollection<T> c, ICollection<U> range) where T: U {
        foreach (U item in range) {
            c.Add((T) item);
        }
    }
}
