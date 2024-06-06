using System.IO;

namespace Midnight;

public interface IAsset : System.IDisposable {
    string Name { get; }
    string[] Filepaths { get; }
    string Filepath { get; }
    bool IsDisposed { get; }

    void Reload();
    void Reload(Stream stream);
}
