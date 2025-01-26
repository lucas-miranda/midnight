using System.IO;

namespace Midnight;

public interface IAsset {
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
    /// Is this asset already released.
    /// </summary>
    /// <remarks>
    /// Useful to verify if asset was already freed and is at invalid state.
    /// </remarks>
    bool IsReleased { get; }

    /// <summary>
    /// Make asset read it's sources and reconstruct it's data.
    /// </summary>
    bool Reload();

    /// <summary>
    /// Make asset read from a stream and reconstruct it's data.
    /// </summary>
    bool Reload(Stream stream);

    /// <summary>
    /// Release asset's managed resources.
    /// </summary>
    bool Release();
}
