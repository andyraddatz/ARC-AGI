namespace AndyARC.Core;

using MathNet.Numerics.LinearAlgebra;
public static class SystemOne
{
    public readonly static IEnumerable<Func<int[][], int[][]>> Actions =
    [
        Transpose,
        DeDupeXY,
    ];
    public static int[][] Transpose(int[][] x)
    {
        // this function should return the transpose of the input array
        // e.g. Transpose([[1, 2, 3], [4, 5, 6]]) => [[1, 4], [2, 5], [3, 6]]
        var xMatrix = Matrix<float>.Build.DenseOfRowArrays(x.Select(row => row.Select(val => (float)val).ToArray()));
        var xTransposed = xMatrix.Transpose();
        return xTransposed.ToColumnArrays().Select(row => row.Select(val => (int)val).ToArray()).ToArray();
    }
    public static int[] DeDupe(int[] x)
    {
        // this function should return an array of unique numbers in the input array
        // in the order they appear in the input array
        // e.g. DeDupe([1, 1, 2, 3, 3, 3, 8, 8, 4]) => [1, 2, 3, 8, 4]
        var y = new List<int>();
        foreach (var val in x)
        {
            if (!y.Contains(val))
            {
                y.Add(val);
            }
        }
        return [.. y];
    }
    public static int[][] DeDupeXY(int[][] xy)
    {
        // this function should return an array of unique numbers in the input array
        // in the order they appear in the input array
        int[][] finalXY = [];
        for (var row = 0; row < xy.Length; row++)
        {
            if (row == 0)
                finalXY = [DeDupe(xy[row])];

            else if (!finalXY[^1].SequenceEqual(DeDupe(xy[row])))
                finalXY = [.. finalXY, DeDupe(xy[row])];
        }

        return finalXY;
    }

}