using Midnight.Diagnostics;
namespace Midnight;

public interface ITextureUniform<T> : ITextureUniform where T : Texture {
    new T Texture { get; set; }

    Texture ITextureUniform.Texture {
        get => Texture;
        set {
            Assert.True(value is T);
            Texture = (T) value;
        }
    }
}

public interface ITextureUniform {
    Texture Texture { get; set; }
}
