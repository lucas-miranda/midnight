using Xna = Microsoft.Xna.Framework;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class ShaderParameter {
    internal ShaderParameter(XnaGraphics.EffectParameter xnaParameter) {
        Debug.AssertNotNull(xnaParameter);
        Underlying = xnaParameter;
    }

    public string Name { get => Underlying.Name; }

    internal XnaGraphics.EffectParameter Underlying { get; }

    public bool GetBool() {
        return Underlying.GetValueBoolean();
    }

    public float GetSingle() {
        return Underlying.GetValueSingle();
    }

    public float[] GetSingleArray(int count) {
        return Underlying.GetValueSingleArray(count);
    }

    public int GetInt32() {
        return Underlying.GetValueInt32();
    }

    public int[] GetInt32Array(int count) {
        return Underlying.GetValueInt32Array(count);
    }

    public Matrix GetMatrix() {
        return new(Underlying.GetValueMatrix());
    }

    public Matrix[] GetMatrixArray(int count) {
        System.Span<Xna.Matrix> value = new(Underlying.GetValueMatrixArray(count));
        System.Span<Matrix> newValue = stackalloc Matrix[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = new(value[i]);
        }

        return newValue.ToArray();
    }

    /*
    public Quaternion GetQuaternion() {
        return Underlying.GetValueQuaternion();
    }
    */

    public Vector2 GetVector2() {
        return new(Underlying.GetValueVector2());
    }

    public Vector2[] GetVector2Array(int count) {
        System.Span<Xna.Vector2> value = new(Underlying.GetValueVector2Array(count));
        System.Span<Vector2> newValue = stackalloc Vector2[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = new(value[i]);
        }

        return newValue.ToArray();
    }

    public Vector3 GetVector3() {
        return new(Underlying.GetValueVector3());
    }

    public Vector3[] GetVector3Array(int count) {
        System.Span<Xna.Vector3> value = new(Underlying.GetValueVector3Array(count));
        System.Span<Vector3> newValue = stackalloc Vector3[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = new(value[i]);
        }

        return newValue.ToArray();
    }

    public Vector4 GetVector4() {
        return new(Underlying.GetValueVector4());
    }

    public Vector4[] GetVector4Array(int count) {
        System.Span<Xna.Vector4> value = new(Underlying.GetValueVector4Array(count));
        System.Span<Vector4> newValue = stackalloc Vector4[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = new(value[i]);
        }

        return newValue.ToArray();
    }

    public string GetString() {
        return Underlying.GetValueString();
    }

    public Texture2D GetTexture2D() {
        if (Underlying == null || Underlying.GetValueTexture2D() == null) {
            return null;
        }

        return new(Underlying.GetValueTexture2D());
    }

    public Color GetColor() {
        return GetColorF().ToByte();
    }

    public ColorF GetColorF() {
        return new(GetVector4());
    }

    public void Set(bool value) {
        Underlying.SetValue(value);
    }

    public void Set(float value) {
        Underlying.SetValue(value);
    }

    public void Set(float[] value) {
        Underlying.SetValue(value);
    }

    public void Set(int value) {
        Underlying.SetValue(value);
    }

    public void Set(int[] value) {
        Underlying.SetValue(value);
    }

    public void Set(Matrix value) {
        Underlying.SetValue(value.ToXna());
    }

    public void Set(Matrix[] value) {
        System.Span<Xna.Matrix> newValue = stackalloc Xna.Matrix[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = value[i].ToXna();
        }

        Underlying.SetValue(newValue.ToArray());
    }

    /*
    public void Set(Quaternion value) {
        Underlying.SetValue(value);
    }
    */

    public void Set(Vector2 value) {
        Underlying.SetValue(value.ToXna());
    }

    public void Set(Vector2[] value) {
        System.Span<Xna.Vector2> newValue = stackalloc Xna.Vector2[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = value[i].ToXna();
        }

        Underlying.SetValue(newValue.ToArray());
    }

    public void Set(Vector3 value) {
        Underlying.SetValue(value.ToXna());
    }

    public void Set(Vector3[] value) {
        System.Span<Xna.Vector3> newValue = stackalloc Xna.Vector3[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = value[i].ToXna();
        }

        Underlying.SetValue(newValue.ToArray());
    }

    public void Set(Vector4 value) {
        Underlying.SetValue(value.ToXna());
    }

    public void Set(Vector4[] value) {
        System.Span<Xna.Vector4> newValue = stackalloc Xna.Vector4[value.Length];

        for (int i = 0; i < value.Length; i++) {
            newValue[i] = value[i].ToXna();
        }

        Underlying.SetValue(newValue.ToArray());
    }

    public void Set(string value) {
        Underlying.SetValue(value);
    }

    public void Set(Texture2D value) {
        Underlying.SetValue(value?.Underlying);
    }

    public void Set(Color value) {
        Set(value.Normalized().ToVec4());
    }

    public void Set(ColorF value) {
        Set(value.ToVec4());
    }
}
