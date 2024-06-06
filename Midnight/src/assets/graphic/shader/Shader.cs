using System.Collections.Generic;
using System.IO;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class Shader : IAsset {
    private Dictionary<string, ShaderParameter> _parameters = new();
    private Dictionary<string, ShaderTechnique> _techniquesByName = new();
    private List<ShaderTechnique> _techniques = new();
    private ShaderTechnique _currentTechnique;

    public Shader() {
    }

    internal Shader(XnaGraphics.Effect xnaEffect) {
        Debug.AssertNotNull(xnaEffect);
        Underlying = xnaEffect;
    }

    public string Name { get; set; }
    public string[] Filepaths { get; protected set; } = new string[1];
    public string Filepath { get => Filepaths[0]; protected set => Filepaths[0] = value; }
    public int TechniqueCount { get => _techniques.Count; }
    public bool IsDisposed { get; private set; }

    public ShaderTechnique CurrentTechnique {
        get => _currentTechnique;
        protected set {
            if (value == _currentTechnique) {
                return;
            }

            _currentTechnique = value;
            Underlying.CurrentTechnique = _currentTechnique?.Underlying;
        }
    }

    internal XnaGraphics.Effect Underlying { get; init; }

    public static S Load<S>(string filepath) where S : Shader, new() {
        using (FileStream stream = File.OpenRead(filepath)) {
            S shader = Load<S>(stream);
            shader.Filepath = filepath;
            shader.Setup();
            return shader;
        }
    }

    public static S Load<S>(byte[] bytecode) where S : Shader, new() {
        Debug.AssertNotNull(Program.Rendering);
        XnaGraphics.Effect xnaEffect = new XnaGraphics.Effect(
            Program.Rendering.XnaGraphicsDevice,
            bytecode
        );

        if (xnaEffect == null) {
            throw new System.InvalidOperationException("Device failed to create shader.");
        }

        S shader = new() {
            Underlying = xnaEffect,
        };

        shader.Setup();
        return shader;
    }

    public static S Load<S>(Stream stream) where S : Shader, new() {
        Debug.AssertNotNull(Program.Rendering);

        if (!stream.CanSeek) {
            throw new System.InvalidOperationException("Can't load, stream can't seek.");
        }

        stream.Seek(0, SeekOrigin.End);
        long size = stream.Position;

        if (size <= 0) {
            throw new System.InvalidOperationException("Stream size is invalid.");
        }

        stream.Seek(0, SeekOrigin.Begin);
        byte[] contents = new byte[size];
        stream.Read(contents);

        XnaGraphics.Effect xnaEffect = new XnaGraphics.Effect(
            Program.Rendering.XnaGraphicsDevice,
            contents
        );

        if (xnaEffect == null) {
            throw new System.InvalidOperationException("Device failed to create shader.");
        }

        S shader = new() {
            Underlying = xnaEffect,
        };

        shader.Setup();
        return shader;
    }

    public IEnumerable<ShaderPass> Apply() {
        Debug.AssertNotNull(CurrentTechnique);
        PreApply();

        foreach (ShaderPass pass in CurrentTechnique.Passes) {
            PrePass();
            yield return pass;
            PassApplied();
        }

        Applied();
    }

    public ShaderTechnique Technique(int id) {
        return _techniques[id];
    }

    public ShaderTechnique Technique(string name) {
        return _techniquesByName[name];
    }

    public ShaderTechnique Technique(System.Enum id) {
        return Technique(System.Convert.ToInt32(id));
    }

    public void ChangeTechnique(int id) {
        CurrentTechnique = _techniques[id];
    }

    public void ChangeTechnique(string name) {
        CurrentTechnique = _techniquesByName[name];
    }

    public void Reload() {
    }

    public void Reload(Stream stream) {
    }

    public void Dispose() {
        IsDisposed = true;
    }

    protected virtual void Loaded() {
    }

    protected virtual void PreApply() {
    }

    protected virtual void Applied() {
    }

    protected virtual void PrePass() {
    }

    protected virtual void PassApplied() {
    }

    protected ShaderParameter RetrieveParameter(string name) {
#if DEBUG
        if (!_parameters.TryGetValue(name, out ShaderParameter parameter)) {
            throw new System.InvalidOperationException($"Shader parameter '{name}' not found.");
        }

        return parameter;
#else
        return _parameters[name];
#endif
    }

    protected ShaderParameter GetParameter(string name) {
        if (!_parameters.TryGetValue(name, out ShaderParameter parameter)) {
            return null;
        }

        return parameter;
    }

    private void Setup() {
        // parameters
        _parameters.Clear();

        foreach (XnaGraphics.EffectParameter xnaParameter in Underlying.Parameters) {
            ShaderParameter parameter = new(xnaParameter);
            _parameters.Add(parameter.Name, parameter);
        }

        // techniques
        for (int i = 0; i < Underlying.Techniques.Count; i++) {
            XnaGraphics.EffectTechnique xnaTechnique = Underlying.Techniques[i];
            ShaderTechnique technique = new(xnaTechnique, i);
            _techniquesByName.Add(technique.Name, technique);
            _techniques.Add(technique);
        }

        // always load first technique
        if (TechniqueCount > 0) {
            ChangeTechnique(0);
        }

        //

        Loaded();
    }
}
