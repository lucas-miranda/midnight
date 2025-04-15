using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Midnight.Assets.Aseprite;

public class AsepriteImporter {
    public enum ChunkType {
        Undefined      = 0,
        OldPalette     = 0x0004,
        OldPaletteHalf = 0x0011,
        Layer          = 0x2004,
        Cel            = 0x2005,
        CelExtra       = 0x2006,
        ColorProfile   = 0x2007,
        ExternalFiles  = 0x2008,
        Mask           = 0x2016,
        Path           = 0x2017,
        Tags           = 0x2018,
        Palette        = 0x2019,
        UserData       = 0x2020,
        Slice          = 0x2022,
        Tileset        = 0x2023,
    }

    public enum LayerType {
        Image = 0,
        Group = 1,
        Tilemap = 2,
    }

    public enum CelType {
        RawImage = 0,
        LinkedCel = 1,
        CompressedImage = 2,
        CompressedTilemap = 3,
    }

    private List<AsepriteLayer> _ancestry = new();
    private Color[] _buffer;
    private byte[] _pixelsBuffer;
    private AsepriteCel _last;
    private long _chunkEndPos;

    public AsepriteImporter() {
    }

    public AsepriteData Data { get; private set; }

    public void Import(PathBuf filepath) {
        Data = new();
        //Logger.Line("Start importing Aseprite file...", "importer");

        using (var fileStream = File.OpenRead(filepath)) {
            using (var reader = new BinaryReader(fileStream, System.Text.Encoding.UTF8, false)) {
                if (!ReadHeader(reader, out ushort frames)) {
                    // failed to read header
                    return;
                }

                for (ushort i = 0; i < frames; i++) {
                    //Logger.Line($"Frame #{i}", "importer");
                    ReadFrame(reader);
                }
            }
        }
        //Logger.Line("Complete importing Aseprite file!", "importer");
    }

    private bool ReadHeader(BinaryReader reader, out ushort frames) {
        uint fileSize = reader.ReadUInt32(); // file size
        long pos = reader.BaseStream.Position;
        reader.BaseStream.Seek(0, SeekOrigin.End);
        long realSize = reader.BaseStream.Position;
        reader.BaseStream.Seek(pos, SeekOrigin.Begin);

        //Logger.Line($"File size: {fileSize}, Real size: {realSize}", "importer");

        ushort magicNumber = reader.ReadUInt16();

        if (magicNumber != 0xA5E0) {
            frames = 0;
            return false;
        }

        frames = reader.ReadUInt16();
        ushort width = reader.ReadUInt16(),
               height = reader.ReadUInt16();

        Data.Size = new(width, height);
        Data.ColorDepth = (AsepriteColorDepth) reader.ReadUInt16();
        Data.Flags = (AsepriteFlags) reader.ReadUInt32();
        reader.ReadUInt16(); // speed DEPRECATED
        reader.ReadUInt32(); // zero
        reader.ReadUInt32(); // zero
        Data.TransparentPaletteIndex = reader.ReadByte();

        //reader.ReadBytes(3); // ignore
        Skip(reader, 3); // ignore

        reader.ReadUInt16(); // number of colors

        byte pixelWidth = reader.ReadByte(),
             pixelHeight = reader.ReadByte();

        Data.PixelSize = new(pixelWidth, pixelHeight);

        short gridX = reader.ReadInt16(),
              gridY = reader.ReadInt16();

        Data.GridPosition = new(gridX, gridY);

        ushort gridWidth = reader.ReadUInt16(),
               gridHeight = reader.ReadUInt16();

        Data.GridSize = new(gridWidth, gridHeight);
        //reader.ReadBytes(84); // future
        Skip(reader, 84); // future
        //Logger.Line("File Header done!", "importer");
        return true;
    }

    private void ReadFrame(BinaryReader reader) {
        // Header
        uint bytes = reader.ReadUInt32(); // bytes in this frame
        ushort magicNumber = reader.ReadUInt16();

        if (magicNumber != 0xF1FA) {
            // skip bytes in this frame
            //Logger.Line($"Skipping frame", "importer");
            reader.BaseStream.Seek(bytes - sizeof(ushort), SeekOrigin.Current);
            return;
        }

        ushort oldChunkCount = reader.ReadUInt16(),
               frameDuration = reader.ReadUInt16();
        Skip(reader, 2); // future
        uint newChunkCount = reader.ReadUInt32();

        // Chunks
        uint chunkCount = newChunkCount == 0 ? oldChunkCount : newChunkCount;

        //Logger.Line($"Reading {chunkCount} chunks...", "importer");
        for (int i = 0; i < chunkCount; i++) {
            ReadFrameChunk(reader);
        }
    }

    private void ReadFrameChunk(BinaryReader reader) {
        uint size = reader.ReadUInt32();
        long chunkEndPos = reader.BaseStream.Position + size - sizeof(uint);
        _chunkEndPos = chunkEndPos;
        ChunkType type = (ChunkType) reader.ReadUInt16();
        bool success = false;
        //Logger.Line($"-> {type}", "importer");

        switch (type) {
            case ChunkType.OldPalette:
            case ChunkType.OldPaletteHalf:
                success = ReadOldPaletteFrameChunk(reader);
                break;
            case ChunkType.Layer:
                success = ReadLayerFrameChunk(reader);
                break;
            case ChunkType.Cel:
                success = ReadCelFrameChunk(reader);
                break;
            case ChunkType.CelExtra:
                success = ReadCelExtraFrameChunk(reader);
                break;
            default:
                break;
        }

        if (!success) {
            // skip chunk
            //Logger.Line("Skipping chunk", "importer");
            reader.BaseStream.Seek(chunkEndPos, SeekOrigin.Begin);
        }
    }

    private bool ReadOldPaletteFrameChunk(BinaryReader reader) {
        ushort packets = reader.ReadUInt16();
        Data.Palette = new();

        for (ushort i = 0; i < packets; i++) {
            byte skipEntriesCount = reader.ReadByte(); // don't really know what to do with that
            ushort colorCount = reader.ReadByte();

            if (colorCount == 0) {
                colorCount = 256;
            }

            for (ushort j = 0; j < colorCount; j++) {
                byte r = reader.ReadByte(),
                     g = reader.ReadByte(),
                     b = reader.ReadByte();

                Data.Palette.Add(new(r, g, b));
            }
        }

        //Logger.Line(Data.Palette, "importer");
        return true;
    }

    private bool ReadLayerFrameChunk(BinaryReader reader) {
        AsepriteLayerFlags flags = (AsepriteLayerFlags) reader.ReadUInt16();
        LayerType type = (LayerType) reader.ReadUInt16();
        AsepriteLayer layer;

        switch (type) {
            case LayerType.Image:
                layer = new AsepriteImageLayer();
                break;
            case LayerType.Group:
                layer = new AsepriteGroupLayer();
                break;
            case LayerType.Tilemap:
                layer = new AsepriteTilemapLayer();
                break;
            default:
                throw new System.InvalidOperationException("Invalid layer type.");
        }

        layer.Flags = flags;
        layer.ChildLevel = reader.ReadUInt16();

        if (layer.ChildLevel >= _ancestry.Count) {
            // new level
            _ancestry.Add(layer);
        } else {
            // replace level
            _ancestry.RemoveRange(layer.ChildLevel, _ancestry.Count - layer.ChildLevel);
            _ancestry.Add(layer);
        }

        if (_ancestry.Count > 1) {
            // register to parent layer
            _ancestry[_ancestry.Count - 2].Children.Add(layer);
        }

        reader.ReadUInt16(); // default layer width in pixels (ignored)
        reader.ReadUInt16(); // default layer height in pixels (ignored)
        layer.BlendMode = (AsepriteBlendMode) reader.ReadUInt16();
        layer.Opacity = reader.ReadByte();
        Skip(reader, 3); // future
        layer.Name = ReadString(reader);

        if (layer is AsepriteTilemapLayer tilemapLayer) {
            tilemapLayer.TilesetIndex = reader.ReadUInt32();
        }

        //Logger.Line(layer, "importer");
        layer.Id = Data.Layers.Count;
        //Logger.Line($"With id: {layer.Id}");
        Data.Layers.Add(layer);
        return true;
    }

    private bool ReadCelFrameChunk(BinaryReader reader) {
        ushort layerIndex = reader.ReadUInt16();
        short posX = reader.ReadInt16(),
              posY = reader.ReadInt16();
        byte opacity = reader.ReadByte();
        CelType type = (CelType) reader.ReadUInt16();
        AsepriteCel cel;

        switch (type) {
            case CelType.RawImage:
                cel = new AsepriteRawImageCel(Data);
                break;
            case CelType.LinkedCel:
                cel = new AsepriteLinkedCel(Data);
                break;
            case CelType.CompressedImage:
                cel = new AsepriteCompressedImageCel(Data);
                break;
            case CelType.CompressedTilemap:
                cel = new AsepriteCompressedTilemapCel(Data);
                break;
            default:
                throw new System.InvalidOperationException("Invalid cel type.");
        }

        cel.Position = new(posX, posY);
        cel.Opacity = opacity;
        cel.ZIndex = reader.ReadInt16();
        Skip(reader, 5);

        switch (cel) {
            case AsepriteRawImageCel rawImageCel:
                {
                    ushort width = reader.ReadUInt16(),
                           height = reader.ReadUInt16();

                    rawImageCel.Texture = new(width, height);
                    PrepareBuffer(width * height);
                    ReadPixels(reader, width, height);
                    rawImageCel.Texture.Write(_buffer, 0, width * height);
                }
                break;
            case AsepriteLinkedCel linkedCel:
                {
                    linkedCel.FromFrame = reader.ReadUInt16();
                }
                break;
            case AsepriteCompressedImageCel compressedImageCel:
                {
                    ushort width = reader.ReadUInt16(),
                           height = reader.ReadUInt16();

                    compressedImageCel.Texture = new(width, height);
                    PrepareBuffer(width * height);
                    ReadPixelsCompressed(reader, width, height);
                    compressedImageCel.Texture.Write(_buffer, 0, width * height);
                    //reader.BaseStream.Seek(_chunkEndPos, SeekOrigin.Begin);
                    //Logger.DebugLine($"chunk end pos: {_chunkEndPos}");
                }
                break;
            case AsepriteCompressedTilemapCel compressedTilemapCel:
                {
                    throw new System.NotImplementedException();
                }
            default:
                break;
        }

        //Logger.Line(cel, "importer");

        // register to layer
        Data.Place(layerIndex, cel);

        _last = cel;
        return true;
    }

    private bool ReadCelExtraFrameChunk(BinaryReader reader) {
        _last.Flags = (AsepriteCelFlags) reader.ReadUInt32();

        float preciseX = FromFixed16(reader.ReadUInt32()),
              preciseY = FromFixed16(reader.ReadUInt32());

        _last.PrecisePosition = new(preciseX, preciseY);

        float width = FromFixed16(reader.ReadUInt32()),
              height = FromFixed16(reader.ReadUInt32());

        _last.Size = new(width, height);
        Skip(reader, 16); // future
        return true;
    }

    private void Skip(BinaryReader reader, long bytes) {
        reader.BaseStream.Seek(bytes, SeekOrigin.Current);
    }

    private long ChunkRemaining(BinaryReader reader) {
        return _chunkEndPos - reader.BaseStream.Position;
    }

    private void PrepareBuffer(int size) {
        if (_buffer == null) {
            _buffer = new Color[size];
        } else {
            System.Array.Resize(ref _buffer, size);
        }
    }

    private string ReadString(BinaryReader reader) {
        ushort length = reader.ReadUInt16();
        System.Span<byte> bytes = stackalloc byte[length];
        reader.Read(bytes);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    private void ReadDataCompressed(BinaryReader reader, System.Span<byte> output, int compressedDataSize) {
        using (var outStream = new MemoryStream(reader.ReadBytes(compressedDataSize))) {
            using (var stream = new ZLibStream(outStream, CompressionMode.Decompress, false)) {
                stream.Read(output);
            }
        }
    }

    private void ReadPixels(BinaryReader reader, ushort width, ushort height) {
        switch (Data.ColorDepth) {
            case AsepriteColorDepth.RGBA:
                {
                    System.Span<byte> px = stackalloc byte[4];

                    for (ushort y = 0; y < height; y++) {
                        for (ushort x = 0; x < width; x++) {
                            reader.Read(px);
                            _buffer[x + y * width] = new(px[0], px[1], px[2], px[3]);
                        }
                    }
                }
                break;
            case AsepriteColorDepth.Grayscale:
                {
                    System.Span<byte> px = stackalloc byte[2];

                    for (ushort y = 0; y < height; y++) {
                        for (ushort x = 0; x < width; x++) {
                            reader.Read(px);
                            _buffer[x + y * width] = new(px[0], px[0], px[0], px[1]);
                        }
                    }
                }
                break;
            case AsepriteColorDepth.Indexed:
                {
                    for (ushort y = 0; y < height; y++) {
                        for (ushort x = 0; x < width; x++) {
                            byte index = reader.ReadByte();
                            _buffer[x + y * width] = Data.Palette[index].Color;
                        }
                    }
                }
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    private void ReadPixelsCompressed(BinaryReader reader, ushort width, ushort height) {
        switch (Data.ColorDepth) {
            case AsepriteColorDepth.RGBA:
                {
                    System.Span<byte> pixels = stackalloc byte[width * height * 4];
                    ReadDataCompressed(reader, pixels, (int) ChunkRemaining(reader));

                    for (ushort y = 0; y < height; y++) {
                        for (ushort x = 0; x < width; x++) {
                            int id = x + y * width,
                                pxId = id * 4;

                            _buffer[id] = new(pixels[pxId], pixels[pxId + 1], pixels[pxId + 2], pixels[pxId + 3]);
                        }
                    }
                }
                break;
            case AsepriteColorDepth.Grayscale:
                {
                    System.Span<byte> pixels = stackalloc byte[width * height * 2];
                    ReadDataCompressed(reader, pixels, (int) ChunkRemaining(reader));

                    for (ushort y = 0; y < height; y++) {
                        for (ushort x = 0; x < width; x++) {
                            int id = x + y * width,
                                pxId = id * 2;

                            _buffer[id] = new(pixels[pxId], pixels[pxId], pixels[pxId], pixels[pxId + 1]);
                        }
                    }
                }
                break;
            case AsepriteColorDepth.Indexed:
                {
                    System.Span<byte> pixels = stackalloc byte[width * height];
                    ReadDataCompressed(reader, pixels, (int) ChunkRemaining(reader));

                    for (ushort y = 0; y < height; y++) {
                        for (ushort x = 0; x < width; x++) {
                            int id = x + y * width;
                            _buffer[id] = Data.Palette[pixels[id]].Color;
                        }
                    }
                }
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    private float FromFixed16(uint n) {
        return n / 65536.0f;
    }
}
