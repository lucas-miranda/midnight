using System.IO;
using Midnight.Diagnostics;
using Midnight.Assets.Aseprite;

namespace Midnight;

public class AsepriteTexture : Texture2D {
    private int _frame;

    private AsepriteTexture(AsepriteData data) : base() {
        Assert.NotNull(data);
        Data = data;
        LoadFrame(0);
        //Logger.Line($"Timeline:\n{data.TimelineToString()}");
    }

    public AsepriteData Data { get; }

    public int FrameIndex {
        get => _frame;
        set => LoadFrame(value);
    }

    public static AsepriteTexture Load(PathBuf filepath) {
        AsepriteImporter importer = new();
        importer.Import(filepath);
        return Load(importer.Data);
    }

    public static AsepriteTexture Load(AsepriteData data) {
        return new(data);
    }

    public new static AsepriteTexture Load(byte[] bytecode) {
        using (MemoryStream stream = new(bytecode, false)) {
            return null;
        }
    }

    public new static AsepriteTexture Load(Stream stream) {
        Assert.NotNull(stream);
        return null;
    }

    private void LoadFrame(int index) {
        if (index < 0) {
            index += Data.Frames.Count;
        }

        Assert.True(Data.Frames.Count > 0);
        Assert.InRange(index, 0, Data.Frames.Count - 1);
        _frame = index;
        Underlying = Data.Frames[index].Extract().Underlying;
        Assert.NotNull(Underlying);
    }
}
