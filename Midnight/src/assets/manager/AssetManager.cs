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
    private Dictionary<System.Type, AssetPage> _library = new();

    public AssetManager() {
        _allowedPageTypes.AddRange(DefaultAllowedPageTypes);

        foreach (System.Type type in _allowedPageTypes) {
            _library.Add(type, new AssetPage(type));
        }
    }

    public IAsset Get(System.Type assetType, string name) {
        AssetPage page = FindPage(assetType);
        Assert.NotNull(page);
        return page.Get(name).Asset;
    }

    public T Get<T>(string name) where T : IAsset {
        return (T) Get(typeof(T), name);
    }

    public IAsset Get(System.Type assetType) {
        AssetPage page = FindPage(assetType);
        Assert.NotNull(page);
        return page.Get(assetType).Asset;
    }

    public T Get<T>() where T : IAsset {
        return (T) Get(typeof(T));
    }

    public bool TryGet(System.Type assetType, string name, out IAsset asset) {
        AssetPage page = FindPage(assetType);

        if (page == null || !page.TryGet(name, out AssetRegistry registry)) {
            asset = null;
            return false;
        }

        asset = registry.Asset;
        return true;
    }

    public bool TryGet<T>(string name, out IAsset asset) where T : IAsset {
        return TryGet(typeof(T), name, out asset);
    }

    public AssetRegistry Register<T>(string name, T asset, bool canOverride = false) where T : IAsset {
        AssetPage page = FindPage(asset.GetType());
        Assert.NotNull(page);
        return page.Register(name, asset, canOverride);
    }

    public AssetRegistry Register<T>(T asset, bool canOverride = false) where T : IAsset {
        return Register(typeof(T).Name, asset, canOverride);
    }

    public void RegisterPage(System.Type assetType) {
        Assert.NotNull(assetType);
        Assert.True(typeof(IAsset).IsAssignableFrom(assetType));
        Assert.False(_allowedPageTypes.Contains(assetType));
        _allowedPageTypes.Add(assetType);
        _library.Add(assetType, new(assetType));
    }

    public void RegisterPage<T>() where T : IAsset {
        RegisterPage(typeof(T));
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
