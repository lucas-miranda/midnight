using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class AssetManager {
    public static readonly System.Type[] DefaultAllowedPageTypes = {
        // graphics
        typeof(Texture),
        typeof(Font),
        typeof(Shader),
    };

    private readonly List<System.Type> _allowedPageTypes = new List<System.Type>();
    private static AssetManager _instance;
    private Dictionary<System.Type, AssetPage> _library = new();

    private AssetManager() {
        _allowedPageTypes.AddRange(DefaultAllowedPageTypes);

        foreach (System.Type type in _allowedPageTypes) {
            _library.Add(type, new AssetPage(type));
        }
    }

    public static IAsset Get(System.Type assetType, string name) {
        AssetPage page = _instance.FindPage(assetType);
        Assert.NotNull(page);
        return page.Get(name).Asset;
    }

    public static T Get<T>(string name) where T : IAsset {
        return (T) Get(typeof(T), name);
    }

    public static IAsset Get(System.Type assetType) {
        AssetPage page = _instance.FindPage(assetType);
        Assert.NotNull(page);
        return page.Get(assetType).Asset;
    }

    public static T Get<T>() where T : IAsset {
        return (T) Get(typeof(T));
    }

    public static bool TryGet(System.Type assetType, string name, out IAsset asset) {
        AssetPage page = _instance.FindPage(assetType);

        if (page == null || !page.TryGet(name, out AssetRegistry registry)) {
            asset = null;
            return false;
        }

        asset = registry.Asset;
        return true;
    }

    public static bool TryGet<T>(string name, out IAsset asset) where T : IAsset {
        return TryGet(typeof(T), name, out asset);
    }

    public static AssetRegistry Register<T>(string name, T asset, bool canOverride = false) where T : IAsset {
        AssetPage page = _instance.FindPage(asset.GetType());
        Assert.NotNull(page);
        return page.Register(name, asset, canOverride);
    }

    public static AssetRegistry Register<T>(T asset, bool canOverride = false) where T : IAsset {
        return Register(typeof(T).Name, asset, canOverride);
    }

    public static void RegisterPage(System.Type assetType) {
        Assert.NotNull(assetType);
        Assert.True(typeof(IAsset).IsAssignableFrom(assetType));
        Assert.False(_instance._allowedPageTypes.Contains(assetType));
        _instance._allowedPageTypes.Add(assetType);
        _instance._library.Add(assetType, new(assetType));
    }

    public static void RegisterPage<T>() where T : IAsset {
        RegisterPage(typeof(T));
    }

    internal static void Initialize() {
        if (_instance != null) {
            return;
        }

        _instance = new();
        _instance.Initialized();
    }

    private void Initialized() {
    }

    private AssetPage FindPage(System.Type assetType) {
        KeyValuePair<System.Type, AssetPage>? candidate = null;

        foreach (KeyValuePair<System.Type, AssetPage> entry in _library) {
            if (entry.Key.IsAssignableFrom(assetType)) {
                if (!candidate.HasValue) {
                    candidate = entry;
                } else if (candidate.Value.Key.IsAssignableFrom(entry.Key)) {
                    // we always prefer a more specific page
                    candidate = entry;
                }
            }
        }

        return candidate?.Value;
    }
}
