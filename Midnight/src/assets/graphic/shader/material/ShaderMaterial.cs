using System.Collections.Generic;

namespace Midnight;

public record class ShaderMaterial(Shader BaseShader) : IShaderUniforms {
    public IEnumerable<ShaderPass> Apply() {
        Applied();
        return BaseShader.Apply();
    }

    public ShaderMaterial Duplicate() {
        return Duplicated();
    }

    protected virtual void Applied() {
    }

    protected virtual ShaderMaterial Duplicated() {
        return new(BaseShader);
    }
}

public abstract record class ShaderMaterial<S>(S Shader)
    : ShaderMaterial(Shader) where S : Shader
{

    protected override abstract ShaderMaterial<S> Duplicated();
}
