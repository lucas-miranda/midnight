using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class AssetPage {
    private Dictionary<System.Guid, AssetRegistry> _registries = new();
    private Dictionary<System.Type, List<AssetRegistry>> _registriesByType = new();

    public AssetPage(System.Type assetType) {
        AssetType = assetType;
    }

    public System.Type AssetType { get; }
    public int Count => _registries.Count;

    public AssetRegistry Get(string name) {
        System.Guid uuid = HashName(name);
        Assert.True(_registries.ContainsKey(uuid));
        return _registries[uuid];
    }

    public AssetRegistry Get(System.Type type) {
        Assert.True(AssetType.IsAssignableFrom(type));
        Assert.True(_registriesByType.ContainsKey(type));
        List<AssetRegistry> entries = _registriesByType[type];
        Assert.False(entries.IsEmpty());
        return entries[0];
    }

    public bool TryGet(string name, out AssetRegistry registry) {
        return _registries.TryGetValue(HashName(name), out registry);
    }

    public bool TryGet(System.Type type, out AssetRegistry registry) {
        if (!_registriesByType.TryGetValue(type, out List<AssetRegistry> entries) || entries.IsEmpty()) {
            registry = null;
            return false;
        }

        registry = entries[0];
        return true;
    }

    public AssetRegistry Register(string name, IAsset asset, bool canOverride = false) {
        Assert.False(string.IsNullOrEmpty(name));
        Assert.NotNull(asset);
        Assert.True(asset.GetType().IsSubclassOf(AssetType), $"Can't register an asset with type '{asset.GetType().Name}' in a page of type '{AssetType}'.");

        System.Guid uuid = HashName(name);
        AssetRegistry registry;

        if (canOverride) {
            if (_registries.TryGetValue(uuid, out registry)) {
                RemoveTypes(registry);
                registry.Asset = asset;
                RegisterTypes(registry);
            } else {
                registry = new(asset, uuid);
                _registries[uuid] = registry;
                RegisterTypes(registry);
            }
        } else {
            Assert.True(!_registries.ContainsKey(uuid));
            registry = new(asset, uuid);
            _registries[uuid] = registry;
            RegisterTypes(registry);
        }

        return registry;
    }

    public List<AssetRegistry> Query<T>() where T : IAsset {
        return Query(typeof(T));
    }

    public List<AssetRegistry> Query(System.Type type) {
        Assert.True(AssetType.IsAssignableFrom(type));
        List<AssetRegistry> result;

        if (_registriesByType.TryGetValue(type, out List<AssetRegistry> entries)) {
            result = new(entries);
        } else {
            result = new();
        }

        return result;
    }

    public bool Contains(string name) {
        return _registries.ContainsKey(HashName(name));
    }

#if !RELEASE
    public int ReloadOutdated() {
        int count = 0;

        foreach (AssetRegistry registry in _registries.Values) {
            if (registry.ReloadOutdated()) {
                count += 1;
            }
        }

        return count;
    }
#endif

    private System.Guid HashName(string name) {
        return UUID.v3.Convert(name);
    }

    private void RegisterTypes(AssetRegistry registry) {
        System.Type type = registry.Asset.GetType();

        while (type != typeof(object)) {
            if (!_registriesByType.TryGetValue(type, out List<AssetRegistry> entries)) {
                entries = new();
                _registriesByType[type] = entries;
            }

            entries.Add(registry);
            type = type.BaseType;
        }
    }

    private void RemoveTypes(AssetRegistry registry) {
        System.Type type = registry.Asset.GetType();

        while (type != typeof(object)) {
            if (!_registriesByType.TryGetValue(type, out List<AssetRegistry> entries)) {
                break;
            }

            entries.Remove(registry);
            type = type.BaseType;
        }
    }
}
