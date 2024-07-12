using System.IO;

namespace Midnight;

public interface IAsset : System.IDisposable {
    /// <summary>
    /// A meaningful identification.
    /// </summary>
    /// <remarks>
    /// It could be a filename, a context identification or a custom name.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// All external files which was used to properly load.
    /// </summary>
    string[] Filepaths { get; }

    /// <summary>
    /// The external file which was used to properly load.
    /// </summary>
    /// <remarks>
    /// If there is multiple filepaths, it'll return only the first.
    /// </remarks>
    public string Filepath { get => Filepaths[0]; protected set => Filepaths[0] = value; }

    /// <summary>
    /// Is this asset already disposed.
    /// </summary>
    /// <remarks>
    /// Useful to verify if asset was already freed and is at invalid state.
    /// </remarks>
    bool IsDisposed { get; }

    /// <summary>
    /// Make asset read it's sources and reconstruct it's data.
    /// </summary>
    void Reload();

    /// <summary>
    /// Make asset read from a stream and reconstruct it's data.
    /// </summary>
    void Reload(Stream stream);
}
