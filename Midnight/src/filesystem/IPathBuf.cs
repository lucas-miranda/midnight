using System.Collections.ObjectModel;

namespace Midnight;

public interface IPathBuf {
    int PartsCount { get; }
    string Name { get; }
    string ParentName { get; }
    bool HasParent { get; }
    System.IO.FileInfo FileInfo { get; }
    System.IO.DirectoryInfo DirectoryInfo { get; }
    ReadOnlyCollection<string> Sections { get; }
    string this[int index] { get; }

    PathBuf Parent();
    bool ExistsFile();
    bool ExistsDirectory();
    bool IsEmpty();
    bool Contains(IPathBuf path);
    bool StartsWith(IPathBuf path);
    PathBuf WithSections(string section);
    PathBuf WithSections(string sectionA, string sectionB);
    PathBuf WithSections(string sectionA, string sectionB, string sectionC);
    PathBuf WithSections(string sectionA, string sectionB, string sectionC, string sectionD);
    string ToString();
}
