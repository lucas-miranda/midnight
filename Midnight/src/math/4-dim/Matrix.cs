using Xna = Microsoft.Xna.Framework;
using Midnight.Diagnostics;

namespace Midnight;

public struct Matrix {
    public static Matrix Identity = new(
            new(1, 0, 0, 0),
            new(0, 1, 0, 0),
            new(0, 0, 1, 0),
            new(0, 0, 0, 1)
        );

    public const int Columns = 4;
    public const int Rows = 4;

    /*
     * Row0 [Row0.X, Row0.Y, Row0.Z, Row0.W]
     * Row1 [Row1.X, Row1.Y, Row1.Z, Row1.W]
     * Row2 [Row2.X, Row2.Y, Row2.Z, Row2.W]
     * Row3 [Row3.X, Row3.Y, Row3.Z, Row3.W]
     */
    public Vector4 Row0, Row1, Row2, Row3;

    public Matrix(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3) {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    public Matrix(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33
    ) {
        Row0 = new(m00, m01, m02, m03);
        Row1 = new(m10, m11, m12, m13);
        Row2 = new(m20, m21, m22, m23);
        Row3 = new(m30, m31, m32, m33);
    }

    public Matrix(System.Span<float> m) {
        Debug.Assert(m.Length >= 4 * 4);
        Row0 = new(m[0], m[1], m[2], m[3]);
        Row1 = new(m[4], m[5], m[6], m[7]);
        Row2 = new(m[8], m[9], m[10], m[11]);
        Row3 = new(m[12], m[13], m[14], m[15]);
    }

    public Matrix(float[] m) : this(new System.Span<float>(m)) {
    }

    internal Matrix(Xna.Matrix xnaMatrix) {
        Row0 = new(xnaMatrix.M11, xnaMatrix.M12, xnaMatrix.M13, xnaMatrix.M14);
        Row1 = new(xnaMatrix.M21, xnaMatrix.M22, xnaMatrix.M23, xnaMatrix.M24);
        Row2 = new(xnaMatrix.M31, xnaMatrix.M32, xnaMatrix.M33, xnaMatrix.M34);
        Row3 = new(xnaMatrix.M41, xnaMatrix.M42, xnaMatrix.M43, xnaMatrix.M44);
    }

    public static Matrix Multiply(ref Matrix a, ref Matrix b) {
        return new(
            a.Row0.X * b.Row0.X + a.Row0.Y * b.Row1.X + a.Row0.Z * b.Row2.X + a.Row0.W * b.Row3.X,
            a.Row0.X * b.Row0.Y + a.Row0.Y * b.Row1.Y + a.Row0.Z * b.Row2.Y + a.Row0.W * b.Row3.Y,
            a.Row0.X * b.Row0.Z + a.Row0.Y * b.Row1.Z + a.Row0.Z * b.Row2.Z + a.Row0.W * b.Row3.Z,
            a.Row0.X * b.Row0.W + a.Row0.Y * b.Row1.W + a.Row0.Z * b.Row2.W + a.Row0.W * b.Row3.W,

            a.Row1.X * b.Row0.X + a.Row1.Y * b.Row1.X + a.Row1.Z * b.Row2.X + a.Row1.W * b.Row3.X,
            a.Row1.X * b.Row0.Y + a.Row1.Y * b.Row1.Y + a.Row1.Z * b.Row2.Y + a.Row1.W * b.Row3.Y,
            a.Row1.X * b.Row0.Z + a.Row1.Y * b.Row1.Z + a.Row1.Z * b.Row2.Z + a.Row1.W * b.Row3.Z,
            a.Row1.X * b.Row0.W + a.Row1.Y * b.Row1.W + a.Row1.Z * b.Row2.W + a.Row1.W * b.Row3.W,

            a.Row2.X * b.Row0.X + a.Row2.Y * b.Row1.X + a.Row2.Z * b.Row2.X + a.Row2.W * b.Row3.X,
            a.Row2.X * b.Row0.Y + a.Row2.Y * b.Row1.Y + a.Row2.Z * b.Row2.Y + a.Row2.W * b.Row3.Y,
            a.Row2.X * b.Row0.Z + a.Row2.Y * b.Row1.Z + a.Row2.Z * b.Row2.Z + a.Row2.W * b.Row3.Z,
            a.Row2.X * b.Row0.W + a.Row2.Y * b.Row1.W + a.Row2.Z * b.Row2.W + a.Row2.W * b.Row3.W,

            a.Row3.X * b.Row0.X + a.Row3.Y * b.Row1.X + a.Row3.Z * b.Row2.X + a.Row3.W * b.Row3.X,
            a.Row3.X * b.Row0.Y + a.Row3.Y * b.Row1.Y + a.Row3.Z * b.Row2.Y + a.Row3.W * b.Row3.Y,
            a.Row3.X * b.Row0.Z + a.Row3.Y * b.Row1.Z + a.Row3.Z * b.Row2.Z + a.Row3.W * b.Row3.Z,
            a.Row3.X * b.Row0.W + a.Row3.Y * b.Row1.W + a.Row3.Z * b.Row2.W + a.Row3.W * b.Row3.W
        );
    }

    public static Matrix Ortho(float width, float height, float near, float far) {
        // ref: https://learn.microsoft.com/en-us/windows/win32/direct3d9/d3dxmatrixorthorh
        return new(
            new(2.0f / width,                       0.0f,                               0.0f,                   0.0f),
            new(0.0f,                               2.0f / height,                      0.0f,                   0.0f),
            new(0.0f,                               0.0f,                               1.0f / (near - far),    0.0f),
            new(0.0f,                               0.0f,                               near / (near - far),    1.0f)
        );
    }

    public static Matrix OrthoOffCenter(float bottom, float top, float left, float right, float near, float far) {
        // ref: https://learn.microsoft.com/en-us/windows/win32/direct3d9/d3dxmatrixorthooffcenterrh
        return new(
            new(2.0f / (right - left),              0.0f,                               0.0f,                   0.0f),
            new(0.0f,                               2.0f / (top - bottom),              0.0f,                   0.0f),
            new(0.0f,                               0.0f,                               1.0f / (near - far),    0.0f),
            new((right + left) / (left - right),    (top + bottom) / (bottom - top),    near / (near - far),    1.0f)
        );
    }

    public static Matrix LookAt(Vector3 eye, Vector3 target, Vector3 up) {
        Vector3 zAxis = (eye - target).Normalized(),
                xAxis = up.Cross(zAxis).Normalized(),
                yAxis = zAxis.Cross(xAxis);

        return new(
            new(xAxis.X,            yAxis.X,            zAxis.X,            0.0f),
            new(xAxis.Y,            yAxis.Y,            zAxis.Y,            0.0f),
            new(xAxis.Z,            yAxis.Z,            zAxis.Z,            0.0f),
            new(-xAxis.Dot(eye),    -yAxis.Dot(eye),    -zAxis.Dot(eye),    1.0f)
        );
    }

    public float Determinant() {
        // using Laplace Expansion
        // ref: https://en.wikipedia.org/wiki/Laplace_expansion

        return Row0.X * (Row1.Y * Row2.Z * Row3.W + Row1.Z * Row2.W * Row3.Y + Row1.W * Row2.Y * Row3.Z - Row1.W * Row2.Z * Row3.Y - Row1.Z * Row2.Y * Row3.W - Row1.Y * Row2.W * Row3.Z)
             - Row0.Y * (Row1.X * Row2.Z * Row3.W + Row1.Z * Row2.W * Row3.X + Row1.W * Row2.X * Row3.Z - Row1.W * Row2.Z * Row3.X - Row1.Z * Row2.X * Row3.W - Row1.X * Row2.W * Row3.Z)
             + Row0.Z * (Row1.X * Row2.Y * Row3.W + Row1.Y * Row2.W * Row3.X + Row1.W * Row2.X * Row3.Y - Row1.W * Row2.Y * Row3.X - Row1.Y * Row2.X * Row3.W - Row1.X * Row2.W * Row3.Y)
             - Row0.W * (Row1.X * Row2.Y * Row3.Z + Row1.Y * Row2.Z * Row3.X + Row1.Z * Row2.X * Row3.Y - Row1.Z * Row2.Y * Row3.X - Row1.Y * Row2.X * Row3.Z - Row1.X * Row2.Z * Row3.Y);
    }

    public Matrix Invert() {
        // use adjugate matrix (transpose of cofactor matrix) to help to calculate inverse of a matrix
        // if matrix is invertible, then, it's inverse matrix is: M^-1 = (1 / |M|) * adj(M)
        //
        // ref: https://en.wikipedia.org/wiki/Invertible_matrix#In_relation_to_its_adjugate

        float d = Determinant();

#if DEBUG
        if (Math.ApproxZero(d)) {
            throw new System.InvalidOperationException("Matrix isn't invertible.");
        }
#endif

        return new(
            (1.0f / d) *  (Row1.Y * Row2.Z * Row3.W + Row1.Z * Row2.W * Row3.Y + Row1.W * Row2.Y * Row3.Z - Row1.W * Row2.Z * Row3.Y - Row1.Z * Row2.Y * Row3.W - Row1.Y * Row2.W * Row3.Z),
            (1.0f / d) * -(Row0.Y * Row2.Z * Row3.W + Row0.Z * Row2.W * Row3.Y + Row0.W * Row2.Y * Row3.Z - Row0.W * Row2.Z * Row3.Y - Row0.Z * Row2.Y * Row3.W - Row0.Y * Row2.W * Row3.Z),
            (1.0f / d) *  (Row0.Y * Row1.Z * Row3.W + Row0.Z * Row1.W * Row3.Y + Row0.W * Row1.Y * Row3.Z - Row0.W * Row1.Z * Row3.Y - Row0.Z * Row1.Y * Row3.W - Row0.Y * Row1.W * Row3.Z),
            (1.0f / d) * -(Row0.Y * Row1.Z * Row2.W + Row0.Z * Row1.W * Row2.Y + Row0.W * Row1.Y * Row2.Z - Row0.W * Row1.Z * Row2.Y - Row0.Z * Row1.Y * Row2.W - Row0.Y * Row1.W * Row2.Z),

            (1.0f / d) * -(Row1.X * Row2.Z * Row3.W + Row1.Z * Row2.W * Row3.X + Row1.W * Row2.X * Row3.Z - Row1.W * Row2.Z * Row3.X - Row1.Z * Row2.X * Row3.W - Row1.X * Row2.W * Row3.Z),
            (1.0f / d) *  (Row0.X * Row2.Z * Row3.W + Row0.Z * Row2.W * Row3.X + Row0.W * Row2.X * Row3.Z - Row0.W * Row2.Z * Row3.X - Row0.Z * Row2.X * Row3.W - Row0.X * Row2.W * Row3.Z),
            (1.0f / d) * -(Row0.X * Row1.Z * Row3.W + Row0.Z * Row1.W * Row3.X + Row0.W * Row1.X * Row3.Z - Row0.W * Row1.Z * Row3.X - Row0.Z * Row1.X * Row3.W - Row0.X * Row1.W * Row3.Z),
            (1.0f / d) *  (Row0.X * Row1.Z * Row2.W + Row0.Z * Row1.W * Row2.X + Row0.W * Row1.X * Row2.Z - Row0.W * Row1.Z * Row2.X - Row0.Z * Row1.X * Row2.W - Row0.X * Row1.W * Row2.Z),

            (1.0f / d) *  (Row1.X * Row2.Y * Row3.W + Row1.Y * Row2.W * Row3.X + Row1.W * Row2.X * Row3.Y - Row1.W * Row2.Y * Row3.X - Row1.Y * Row2.X * Row3.W - Row1.X * Row2.W * Row3.Y),
            (1.0f / d) * -(Row0.X * Row2.Y * Row3.W + Row0.Y * Row2.W * Row3.X + Row0.W * Row2.X * Row3.Y - Row0.W * Row2.Y * Row3.X - Row0.Y * Row2.X * Row3.W - Row0.X * Row2.W * Row3.Y),
            (1.0f / d) *  (Row0.X * Row1.Y * Row3.W + Row0.Y * Row1.W * Row3.X + Row0.W * Row1.X * Row3.Y - Row0.W * Row1.Y * Row3.X - Row0.Y * Row1.X * Row3.W - Row0.X * Row1.W * Row3.Y),
            (1.0f / d) * -(Row0.X * Row1.Y * Row2.W + Row0.Y * Row1.W * Row2.X + Row0.W * Row1.X * Row2.Y - Row0.W * Row1.Y * Row2.X - Row0.Y * Row1.X * Row2.W - Row0.X * Row1.W * Row2.Y),

            (1.0f / d) * -(Row1.X * Row2.Y * Row3.Z + Row1.Y * Row2.Z * Row3.X + Row1.Z * Row2.X * Row3.Y - Row1.Z * Row2.Y * Row3.X - Row1.Y * Row2.X * Row3.Z - Row1.X * Row2.Z * Row3.Y),
            (1.0f / d) *  (Row0.X * Row2.Y * Row3.Z + Row0.Y * Row2.Z * Row3.X + Row0.Z * Row2.X * Row3.Y - Row0.Z * Row2.Y * Row3.X - Row0.Y * Row2.X * Row3.Z - Row0.X * Row2.Z * Row3.Y),
            (1.0f / d) * -(Row0.X * Row1.Y * Row3.Z + Row0.Y * Row1.Z * Row3.X + Row0.Z * Row1.X * Row3.Y - Row0.Z * Row1.Y * Row3.X - Row0.Y * Row1.X * Row3.Z - Row0.X * Row1.Z * Row3.Y),
            (1.0f / d) *  (Row0.X * Row1.Y * Row2.Z + Row0.Y * Row1.Z * Row2.X + Row0.Z * Row1.X * Row2.Y - Row0.Z * Row1.Y * Row2.X - Row0.Y * Row1.X * Row2.Z - Row0.X * Row1.Z * Row2.Y)
        );
    }

    public Matrix Cofactor() {
        // calculate cofactor matrix (or matrix of cofactors or comatrix)
        // ref: https://www.cuemath.com/algebra/cofactor-matrix/

        return new(
             (Row1.Y * Row2.Z * Row3.W + Row1.Z * Row2.W * Row3.Y + Row1.W * Row2.Y * Row3.Z - Row1.W * Row2.Z * Row3.Y - Row1.Z * Row2.Y * Row3.W - Row1.Y * Row2.W * Row3.Z),
            -(Row1.X * Row2.Z * Row3.W + Row1.Z * Row2.W * Row3.X + Row1.W * Row2.X * Row3.Z - Row1.W * Row2.Z * Row3.X - Row1.Z * Row2.X * Row3.W - Row1.X * Row2.W * Row3.Z),
             (Row1.X * Row2.Y * Row3.W + Row1.Y * Row2.W * Row3.X + Row1.W * Row2.X * Row3.Y - Row1.W * Row2.Y * Row3.X - Row1.Y * Row2.X * Row3.W - Row1.X * Row2.W * Row3.Y),
            -(Row1.X * Row2.Y * Row3.Z + Row1.Y * Row2.Z * Row3.X + Row1.Z * Row2.X * Row3.Y - Row1.Z * Row2.Y * Row3.X - Row1.Y * Row2.X * Row3.Z - Row1.X * Row2.Z * Row3.Y),

            -(Row0.Y * Row2.Z * Row3.W + Row0.Z * Row2.W * Row3.Y + Row0.W * Row2.Y * Row3.Z - Row0.W * Row2.Z * Row3.Y - Row0.Z * Row2.Y * Row3.W - Row0.Y * Row2.W * Row3.Z),
             (Row0.X * Row2.Z * Row3.W + Row0.Z * Row2.W * Row3.X + Row0.W * Row2.X * Row3.Z - Row0.W * Row2.Z * Row3.X - Row0.Z * Row2.X * Row3.W - Row0.X * Row2.W * Row3.Z),
            -(Row0.X * Row2.Y * Row3.W + Row0.Y * Row2.W * Row3.X + Row0.W * Row2.X * Row3.Y - Row0.W * Row2.Y * Row3.X - Row0.Y * Row2.X * Row3.W - Row0.X * Row2.W * Row3.Y),
             (Row0.X * Row2.Y * Row3.Z + Row0.Y * Row2.Z * Row3.X + Row0.Z * Row2.X * Row3.Y - Row0.Z * Row2.Y * Row3.X - Row0.Y * Row2.X * Row3.Z - Row0.X * Row2.Z * Row3.Y),

             (Row0.Y * Row1.Z * Row3.W + Row0.Z * Row1.W * Row3.Y + Row0.W * Row1.Y * Row3.Z - Row0.W * Row1.Z * Row3.Y - Row0.Z * Row1.Y * Row3.W - Row0.Y * Row1.W * Row3.Z),
            -(Row0.X * Row1.Z * Row3.W + Row0.Z * Row1.W * Row3.X + Row0.W * Row1.X * Row3.Z - Row0.W * Row1.Z * Row3.X - Row0.Z * Row1.X * Row3.W - Row0.X * Row1.W * Row3.Z),
             (Row0.X * Row1.Y * Row3.W + Row0.Y * Row1.W * Row3.X + Row0.W * Row1.X * Row3.Y - Row0.W * Row1.Y * Row3.X - Row0.Y * Row1.X * Row3.W - Row0.X * Row1.W * Row3.Y),
            -(Row0.X * Row1.Y * Row3.Z + Row0.Y * Row1.Z * Row3.X + Row0.Z * Row1.X * Row3.Y - Row0.Z * Row1.Y * Row3.X - Row0.Y * Row1.X * Row3.Z - Row0.X * Row1.Z * Row3.Y),

            -(Row0.Y * Row1.Z * Row2.W + Row0.Z * Row1.W * Row2.Y + Row0.W * Row1.Y * Row2.Z - Row0.W * Row1.Z * Row2.Y - Row0.Z * Row1.Y * Row2.W - Row0.Y * Row1.W * Row2.Z),
             (Row0.X * Row1.Z * Row2.W + Row0.Z * Row1.W * Row2.X + Row0.W * Row1.X * Row2.Z - Row0.W * Row1.Z * Row2.X - Row0.Z * Row1.X * Row2.W - Row0.X * Row1.W * Row2.Z),
            -(Row0.X * Row1.Y * Row2.W + Row0.Y * Row1.W * Row2.X + Row0.W * Row1.X * Row2.Y - Row0.W * Row1.Y * Row2.X - Row0.Y * Row1.X * Row2.W - Row0.X * Row1.W * Row2.Y),
             (Row0.X * Row1.Y * Row2.Z + Row0.Y * Row1.Z * Row2.X + Row0.Z * Row1.X * Row2.Y - Row0.Z * Row1.Y * Row2.X - Row0.Y * Row1.X * Row2.Z - Row0.X * Row1.Z * Row2.Y)
        );
    }

    public Matrix RowEchelonForm() {
        System.Span<float> m = AsSpan();

        int c = 0,
            r = 0;

        while (c < Columns && r < Rows) {
            int rMax = r,
                i;

            for (i = r + 1; i < Rows; i++) {
                if (Math.Abs(m[i * Columns + c]) > Math.Abs(m[rMax * Columns + c])) {
                    rMax = i;
                }
            }

            if (Math.ApproxZero(m[rMax * Columns + c])) {
                c += 1;
            } else {
                System.Span<float> prevRow = new(new float[Columns]);
                m.Slice(r * Columns + 0, Columns).CopyTo(prevRow);
                m.Slice(rMax * Columns + 0, Columns).CopyTo(m.Slice(r * Columns + 0, Columns));
                prevRow.Slice(0, Columns).CopyTo(m.Slice(rMax * Columns + 0, Columns));

                for (i = r + 1; i < Rows; i++) {
                    float f = m[i * Columns + c] / m[r * Columns + c];
                    m[i * Columns + c] = 0.0f;

                    for (int j = c + 1; j < Columns; j++) {
                        m[i * Columns + j] -= m[r * Columns + j] * f;
                    }
                }

                c += 1;
                r += 1;
            }
        }

        return new(m);
    }

    public float[] AsArray() {
        return new float[] {
            Row0.X, Row0.Y, Row0.Z, Row0.W,
            Row1.X, Row1.Y, Row1.Z, Row1.W,
            Row2.X, Row2.Y, Row2.Z, Row2.W,
            Row3.X, Row3.Y, Row3.Z, Row3.W,
        };
    }

    public System.Span<float> AsSpan() {
        return new(new float[] {
            Row0.X, Row0.Y, Row0.Z, Row0.W,
            Row1.X, Row1.Y, Row1.Z, Row1.W,
            Row2.X, Row2.Y, Row2.Z, Row2.W,
            Row3.X, Row3.Y, Row3.Z, Row3.W,
        });
    }

    public override string ToString() {
        return $"{Row0},   {Row1},   {Row2},   {Row3}";
    }

    public static Matrix operator *(Matrix a, Matrix b) {
        return Multiply(ref a, ref b);
    }

    public static Vector3 operator *(Matrix m, Vector3 v) {
        return new(
            m.Row0.X * v.X + m.Row0.Y * v.Y + m.Row0.Z * v.Z + m.Row0.W * 1.0f,
            m.Row1.X * v.X + m.Row1.Y * v.Y + m.Row1.Z * v.Z + m.Row1.W * 1.0f,
            m.Row2.X * v.X + m.Row2.Y * v.Y + m.Row2.Z * v.Z + m.Row2.W * 1.0f
        );
    }

    public static Vector3 operator *(Vector3 v, Matrix m) {
        return new(
            m.Row0.X * v.X + m.Row1.X * v.Y + m.Row2.X * v.Z + m.Row3.X * 1.0f,
            m.Row0.Y * v.X + m.Row1.Y * v.Y + m.Row2.Y * v.Z + m.Row3.Y * 1.0f,
            m.Row0.Z * v.X + m.Row1.Z * v.Y + m.Row2.Z * v.Z + m.Row3.Z * 1.0f
        );
    }

    internal Xna.Matrix ToXna() {
        return new(
            Row0.X, Row0.Y, Row0.Z, Row0.W,
            Row1.X, Row1.Y, Row1.Z, Row1.W,
            Row2.X, Row2.Y, Row2.Z, Row2.W,
            Row3.X, Row3.Y, Row3.Z, Row3.W
        );
    }
}
