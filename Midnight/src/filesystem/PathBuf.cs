using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class PathBuf : IPathBuf {
    private static StringBuilder Builder = new StringBuilder();

    private List<string> _sections;
    private string _result;

    public PathBuf() {
        _sections = new List<string>();
        Sections = _sections.AsReadOnly();
    }

    public PathBuf(string path) : this() {
        Assert.NotNull(path);
        Push(path);
    }

    public PathBuf(string pathA, string pathB) : this() {
        Assert.NotNull(pathA);
        Assert.NotNull(pathB);
        Push(pathA);
        Push(pathB);
    }

    public PathBuf(string pathA, string pathB, string pathC) : this() {
        Assert.NotNull(pathA);
        Assert.NotNull(pathB);
        Assert.NotNull(pathC);
        Push(pathA);
        Push(pathB);
        Push(pathC);
    }

    public PathBuf(string pathA, string pathB, string pathC, string pathD) : this() {
        Assert.NotNull(pathA);
        Assert.NotNull(pathB);
        Assert.NotNull(pathC);
        Assert.NotNull(pathD);
        Push(pathA);
        Push(pathB);
        Push(pathC);
        Push(pathD);
    }

    public PathBuf(IPathBuf path) {
        Assert.NotNull(path);
        _sections = new List<string>(path.Sections);
        Sections = _sections.AsReadOnly();
        _result = path.ToString();
    }

    public int PartsCount => _sections.Count;
    public string Name => _sections.Count == 0 ? null : _sections[_sections.Count - 1];
    public string ParentName => !HasParent ? null : _sections[_sections.Count - 2];
    public bool HasParent => _sections.Count > 1;
    public System.IO.FileInfo FileInfo => new System.IO.FileInfo(ToString());
    public System.IO.DirectoryInfo DirectoryInfo => new System.IO.DirectoryInfo(ToString());
    public ReadOnlyCollection<string> Sections { get; }
    public string this[int index] => _sections[index];

    public PathBuf Push(string path) {
        Assert.NotNull(path);

        if (path.Length == 0) {
            return this;
        }

        _result = null;
        foreach (string section in Split(path, !Sections.IsEmpty())) {
            _sections.Add(section);
        }

        return this;
    }

    public PathBuf Push(IPathBuf path) {
        Assert.NotNull(path);

        if (path.Sections.Count == 0) {
            return this;
        }

        _result = null;
        _sections.AddRange(path.Sections);
        return this;
    }

    public PathBuf PushSections(string section) {
        if (!IsValidSection(section)) {
            throw new System.ArgumentException($"Section '{section}' is invalid", nameof(section));
        }

        _result = null;
        _sections.Add(section);
        return this;
    }

    public PathBuf PushSections(string sectionA, string sectionB) {
        if (!IsValidSection(sectionA)) {
            throw new System.ArgumentException($"Section (0) '{sectionA}' is invalid", nameof(sectionA));
        }

        if (!IsValidSection(sectionB)) {
            throw new System.ArgumentException($"Section (1) '{sectionB}' is invalid", nameof(sectionB));
        }

        _result = null;
        _sections.Add(sectionA);
        _sections.Add(sectionB);
        return this;
    }

    public PathBuf PushSections(string sectionA, string sectionB, string sectionC) {
        if (!IsValidSection(sectionA)) {
            throw new System.ArgumentException($"Section (0) '{sectionA}' is invalid", nameof(sectionA));
        }

        if (!IsValidSection(sectionB)) {
            throw new System.ArgumentException($"Section (1) '{sectionB}' is invalid", nameof(sectionB));
        }

        if (!IsValidSection(sectionC)) {
            throw new System.ArgumentException($"Section (2) '{sectionC}' is invalid", nameof(sectionC));
        }

        _result = null;
        _sections.Add(sectionA);
        _sections.Add(sectionB);
        _sections.Add(sectionC);
        return this;
    }

    public PathBuf PushSections(string sectionA, string sectionB, string sectionC, string sectionD) {
        if (!IsValidSection(sectionA)) {
            throw new System.ArgumentException($"Section (0) '{sectionA}' is invalid", nameof(sectionA));
        }

        if (!IsValidSection(sectionB)) {
            throw new System.ArgumentException($"Section (1) '{sectionB}' is invalid", nameof(sectionB));
        }

        if (!IsValidSection(sectionC)) {
            throw new System.ArgumentException($"Section (2) '{sectionC}' is invalid", nameof(sectionC));
        }

        if (!IsValidSection(sectionD)) {
            throw new System.ArgumentException($"Section (3) '{sectionD}' is invalid", nameof(sectionD));
        }

        _result = null;
        _sections.Add(sectionA);
        _sections.Add(sectionB);
        _sections.Add(sectionC);
        _sections.Add(sectionD);
        return this;
    }

    public string Pop() {
        Assert.True(_sections.Count != 0, "Path is already empty.");
        string lastPart = _sections[_sections.Count - 1];
        _sections.RemoveAt(_sections.Count - 1);
        return lastPart;
    }

    public PathBuf Parent() {
        if (!HasParent) {
            return null;
        }

        PathBuf parentPath = new PathBuf((IPathBuf) this);
        parentPath.Pop();
        return parentPath;
    }

    public bool ExistsFile() {
        return !IsEmpty() && System.IO.File.Exists(ToString());
    }

    public bool ExistsDirectory() {
        return !IsEmpty() && System.IO.Directory.Exists(ToString());
    }

    public bool IsEmpty() {
        return _sections.Count == 0;
    }

    public bool Contains(IPathBuf path) {
        Assert.NotNull(path);
        bool contains = false;

        for (int i = 0; i < PartsCount && i + path.PartsCount <= PartsCount; i++) {
            contains = true;

            for (int j = 0; j < path.PartsCount; j++) {
                if (path[j] != _sections[i + j]) {
                    contains = false;
                    break;
                }
            }

            if (contains) {
                break;
            }
        }

        return contains;
    }

    public bool StartsWith(IPathBuf path) {
        Assert.NotNull(path);

        if (path.PartsCount > PartsCount) {
            return false;
        }

        for (int i = 0; i < path.PartsCount; i++) {
            if (_sections[i] != path[i]) {
                return false;
            }
        }

        return true;
    }

    public bool Remove(IPathBuf path) {
        Assert.NotNull(path);

        if (path.PartsCount > PartsCount) {
            return false;
        }

        // find start index
        int startIndex = -1;
        for (int i = 0; i < PartsCount; i++) {
            if (_sections[i] == path[0]) {
                startIndex = i;
                break;
            }
        }

        if (startIndex < 0) {
            return false;
        }

        // verify all parts from path matches
        for (int i = 0; i < path.PartsCount; i++) {
            if (_sections[startIndex + i] != path[i]) {
                return false;
            }
        }

        _sections.RemoveRange(startIndex, path.PartsCount);
        return true;
    }

    public PathBuf WithSections(string section) {
        PathBuf path = new((IPathBuf) this);
        path.PushSections(section);
        return path;
    }

    public PathBuf WithSections(string sectionA, string sectionB) {
        PathBuf path = new((IPathBuf) this);
        path.PushSections(sectionA, sectionB);
        return path;
    }

    public PathBuf WithSections(string sectionA, string sectionB, string sectionC) {
        PathBuf path = new((IPathBuf) this);
        path.PushSections(sectionA, sectionB, sectionC);
        return path;
    }

    public PathBuf WithSections(string sectionA, string sectionB, string sectionC, string sectionD) {
        PathBuf path = new((IPathBuf) this);
        path.PushSections(sectionA, sectionB, sectionC, sectionD);
        return path;
    }

    public ReadOnlyPathBuf AsReadOnly() {
        return new(this);
    }

    public override string ToString() {
        if (_sections.Count == 0) {
            return string.Empty;
        } else if (_result != null) {
            return _result;
        }

        Builder.Clear();

        for (int i = 0; i < _sections.Count - 1; i++) {
            string section = _sections[i];

            if (!Path.IsValid(section)) {
                throw new System.InvalidOperationException($"Path building failed. Section '{section}' has invalid path chars.");
            }

            Builder.Append(section);
            Builder.Append(Path.Separator);
        }

        Builder.Append(_sections[_sections.Count - 1]);

        _result = Builder.ToString();
        return _result;
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is PathBuf pathBuf && this == pathBuf;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public static implicit operator PathBuf(string path) {
        return new PathBuf(path);
    }

    public static implicit operator string(PathBuf path) {
        return path.ToString();
    }

    public static bool operator ==(PathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null) || PathBuf.ReferenceEquals(r, null)) {
            return PathBuf.ReferenceEquals(l, null) == PathBuf.ReferenceEquals(r, null);
        }

        if (l._sections.Count != r.Sections.Count) {
            return false;
        }

        for (int i = 0; i < l._sections.Count; i++) {
            if (l._sections[i] != r.Sections[i]) {
                return false;
            }
        }

        return true;
    }

    public static bool operator !=(PathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null) || PathBuf.ReferenceEquals(r, null)) {
            return PathBuf.ReferenceEquals(l, null) != PathBuf.ReferenceEquals(r, null);
        }

        if (l._sections.Count != r.Sections.Count) {
            return true;
        }

        for (int i = 0; i < l._sections.Count; i++) {
            if (l._sections[i] != r.Sections[i]) {
                return true;
            }
        }

        return false;
    }

    public static PathBuf operator +(PathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            return new(r);
        }

        return new PathBuf((IPathBuf) l).Push(r);
    }

    public static PathBuf operator +(PathBuf l, string r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            return new(r);
        }

        return new PathBuf((IPathBuf) l).Push(r);
    }

    public static PathBuf operator -(PathBuf l, IPathBuf r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            throw new System.InvalidOperationException();
        }

        PathBuf path = new PathBuf((IPathBuf) l);
        path.Remove(r);
        return path;
    }

    public static PathBuf operator -(PathBuf l, string r) {
        if (PathBuf.ReferenceEquals(l, null)) {
            throw new System.InvalidOperationException();
        }

        PathBuf path = new PathBuf((IPathBuf) l);
        path.Remove(new PathBuf(r));
        return path;
    }

    /// <summary>
    /// Receives a path and split it at it's separators.
    /// </summary>
    /// <param name="value">Path to be splitted.</param>
    /// <param name="canStartTrimSeparator">
    /// Should we trim any separators at beginning of path?
    /// By using this we can accept paths starting with separator without misuderstanding it with root at some systems (e.g Linux).
    /// </param>
    /// <returns>
    /// Each section of path splitted by it's separators.
    /// </returns>
    private static IEnumerable<string> Split(string value, bool canStartTrimSeparator) {
        if (value == null) {
            yield break;
        }

        if (canStartTrimSeparator) {
            // by trimming separator at start, we can accept input which contains separator
            // without mistaking it by root at some systems (e.g Linux)
            value = value.TrimStart(Path.Separators);
        }

        string root = System.IO.Path.GetPathRoot(value);
        if (!string.IsNullOrEmpty(root)) {
            yield return root.TrimEnd(Path.Separators);

            if (value.Length <= root.Length) {
                yield break;
            }

            value = value.Substring(root.Length);
        }

        int start = 0;
        for (int i = 0; i < value.Length; i++) {
            if (Path.IsSeparator(value[i])) {
                if (i - start <= 0) {
                    start = i + 1;
                    continue;
                }

                yield return value.Substring(start, i - start);
                start = i + 1;
            }
        }

        if (start < value.Length) {
            yield return value.Substring(start, (value.Length - 1) - start + 1);
        }
    }

    private static bool HasSeparator(string section) {
        foreach (char c in section) {
            foreach (char sep in Path.Separators) {
                if (c == sep) {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsValidSection(string section) {
        return !(string.IsNullOrWhiteSpace(section) || HasSeparator(section));
    }
}
