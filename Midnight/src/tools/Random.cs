using System.Collections.Generic;

namespace Midnight;

public static class Random {
    private static System.Type BaseRandomType = typeof(System.Random);

    public static bool Initialized { get; private set; }
    public static System.Random Underlying { get; private set; }
    public static int Seed { get; private set; }

    public static void Init<R>(int? customSeed = null) where R : System.Random {
        if (Initialized) {
            return;
        }

        Initialized = true;
        Seed = customSeed.GetValueOrDefault((int) System.DateTime.Now.Ticks);
        Underlying = (System.Random) System.Activator.CreateInstance(typeof(R), Seed);
    }

    public static void Init(int? customSeed = null) {
        if (Initialized) {
            return;
        }

        Initialized = true;
        Seed = customSeed.GetValueOrDefault((int) System.DateTime.Now.Ticks);
        Underlying = new System.Random(Seed);
    }

    public static int Int32() {
        return Underlying.Next();
    }

    public static int Int32(int min, int max) {
        if (max < min) {
            throw new System.ArgumentException($"{nameof(max)} should be greater or equals {nameof(min)}.\nMin: {min}, Max: {max}");
        } else if (min == max) {
            return max;
        }

        return Underlying.Next(min, max + 1);
    }

    public static void Shuffle<T>(IList<T> list) {
        if (list.Count <= 1) {
            return;
        }

        // using Fisher-Yates shuffle algorithm
        int j;
        T value;

        for (int i = list.Count - 1; i >= 0; i--) {
            j = Random.Int32(0, i);
            value = list[i];
            list[i] = list[j];
            list[j] = value;
        }
    }

    public static T Choose<T>(IList<T> list) {
        if (list.Count == 0) {
            throw new System.ArgumentException($"Can't choose an element from an empty IList<{typeof(T)}>");
        } else if (list.Count == 1) {
            return list[0];
        }

        return list[Int32(0, list.Count - 1)];
    }

    public static T Choose<T>(ICollection<T> collection) {
        if (collection.Count == 0) {
            throw new System.ArgumentException($"Can't choose an element from a empty ICollection<{typeof(T)}>");
        }

        IEnumerator<T> enumerator = collection.GetEnumerator();
        int i = Int32(0, collection.Count - 1);

        while (enumerator.MoveNext()) {
            if (i == 0) {
                return enumerator.Current;
            }

            i--;
        }

        return default;
    }
}
