using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class ReadOnlyPathBuf : IPathBuf {
    private readonly PathBuf _path;

    public ReadOnlyPathBuf(PathBuf path) {
        Assert.NotNull(path);
        _path = path;
    }

    public int PartsCount => _path.PartsCount;
    public string Name => _path.Name;
    public string ParentName => _path.ParentName;
    public bool HasParent => _path.HasParent;
    public System.IO.FileInfo FileInfo => _path.FileInfo;
    public System.IO.DirectoryInfo DirectoryInfo => _path.DirectoryInfo;
    public ReadOnlyCollection<string> Sections => _path.Sections;
    public string this[int index] => _path[index];

    public PathBuf Parent() {
        return _path.Parent();
    }

    public bool ExistsFile() {
        return _path.ExistsFile();
    }

    public bool ExistsDirectory() {
        return _path.ExistsDirectory();
    }

    public bool IsEmpty() {
        return _path.IsEmpty();
    }

    public bool Contains(IPathBuf path) {
        return _path.Contains(path);
    }

    public bool StartsWith(IPathBuf path) {
        return _path.StartsWith(path);
    }

    public PathBuf WithSections(string section) {
        return _path.WithSections(section);
    }

    public PathBuf WithSections(string sectionA, string sectionB) {
        return _path.WithSections(sectionA, sectionB);
    }

    public PathBuf WithSections(string sectionA, string sectionB, string sectionC) {
        return _path.WithSections(sectionA, sectionB, sectionC);
    }

    public PathBuf WithSections(string sectionA, string sectionB, string sectionC, string sectionD) {
        return _path.WithSections(sectionA, sectionB, sectionC, sectionD);
    }

    public override string ToString() {
        return _path.ToString();
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is PathBuf pathBuf && this == pathBuf;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public static implicit operator ReadOnlyPathBuf(string path) {
        return new PathBuf(path).AsReadOnly();
    }

    public static implicit operator ReadOnlyPathBuf(PathBuf path) {
        return path.AsReadOnly();
    }

    public static bool operator ==(ReadOnlyPathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null) || PathBuf.ReferenceEquals(r, null)) {
            return PathBuf.ReferenceEquals(l, null) == PathBuf.ReferenceEquals(r, null);
        }

        return l._path == r;
    }

    public static bool operator !=(ReadOnlyPathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null) || PathBuf.ReferenceEquals(r, null)) {
            return PathBuf.ReferenceEquals(l, null) != PathBuf.ReferenceEquals(r, null);
        }

        return l._path != r;
    }

    public static PathBuf operator +(ReadOnlyPathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            return new(r);
        }

        return l._path + r;
    }

    public static PathBuf operator +(ReadOnlyPathBuf l, string r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            return new(r);
        }

        return l._path + r;
    }

    public static PathBuf operator -(ReadOnlyPathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            throw new System.InvalidOperationException();
        }

        return l._path - r;
    }

    public static PathBuf operator -(ReadOnlyPathBuf l, string r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            throw new System.InvalidOperationException();
        }

        return l._path - r;
    }
}
