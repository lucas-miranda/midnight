using System.IO;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class AssetRegistry {
#if !RELEASE
    public System.Action<IAsset> OnReload;
#endif

    private IAsset _asset;

    public AssetRegistry(IAsset asset, System.Guid uuid) {
        Assert.NotNull(asset);
        Asset = asset;
        UUID = uuid;

#if !RELEASE
        if (asset.Filepaths != null) {
            Files = new FileRecord[asset.Filepaths.Length];

            for (int i = 0; i < asset.Filepaths.Length; i++) {
                Files[i] = FileRecord.Create(asset.Filepaths[i]);
            }
        }
#endif
    }

    public System.Guid UUID { get; }
    public FileRecord[] Files { get; }

    public IAsset Asset {
        get => _asset;
        set {
            Assert.NotNull(value);
            _asset = value;
        }
    }

    public void Release() {
        if (Asset != null) {
            Asset.Release();
            Asset = null;
        }

#if !RELEASE
        OnReload = null;
#endif
    }

#if !RELEASE
    public bool Reload() {
        if (!Asset.Reload()) {
            return false;
        }

        OnReload?.Invoke(Asset);
        return true;
    }

    public bool Reload(Stream assetStream) {
        if (!Asset.Reload(assetStream)) {
            return false;
        }

        OnReload?.Invoke(Asset);
        return true;
    }

    public bool ReloadOutdated() {
        if (Files == null || Files.Length == 0) {
            // there is no file registered to be reloaded
            return false;
        }

        bool hasOutdatedFiles = false;
        System.DateTime now = System.DateTime.Now;

        foreach (FileRecord file in Files) {
            file.Refresh();

            if (file.IsUpdated) {
                continue;
            }

            System.TimeSpan timeDiff = now.Subtract(file.Info.LastWriteTime);

            if (timeDiff.Milliseconds >= 500) {
                // we should have to wait a bit before trying to reload the file
                // by doing that we give SO the time to properly write to the disk
                // and we'll never try to reload a incomplete file
                hasOutdatedFiles = true;
                break;
            }
        }

        if (!hasOutdatedFiles) {
            // there is no need to reload, nothing is outdated
            return true;
        }

        if (!Reload()) {
            // reload failed somehow
            return false;
        }

        Update();
        return true;
    }

    public void Update() {
        if (Files == null) {
            return;
        }

        foreach (FileRecord file in Files) {
            file.Update();
        }
    }

    public class FileRecord {
        public FileRecord(FileInfo info) {
            Assert.NotNull(info);
            Info = info;
            LastWriteTime = Info.LastWriteTime;
        }

        public FileInfo Info { get; }
        public System.DateTime? LastWriteTime { get; private set; }
        public bool IsUpdated => LastWriteTime != null && Info.LastWriteTime.CompareTo(LastWriteTime.Value) == 0;
        public bool IsOutdated => !IsUpdated;

        public static FileRecord Create(string filepath) {
            if (string.IsNullOrEmpty(filepath)) {
                return null;
            }

            FileInfo info = new(filepath);

            if (!info.Exists) {
                return null;
            }

            return new(info);
        }

        public void Update() {
            if (!Info.Exists) {
                LastWriteTime = null;
                return;
            }

            LastWriteTime = Info.LastWriteTime;
        }

        public void Refresh() {
            Info.Refresh();
        }
    }
#endif
}
