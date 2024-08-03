namespace AndyARC.Core;

using AndyARC.Core.Features;
using MathNet.Numerics.LinearAlgebra;
public static class SystemOne
{
    public readonly static IEnumerable<Func<int[][], int[][]>> Actions =
    [
        Transpose,
        DeDupeXY,
    ];
    public static int[][] PaintBorder(int[][] x, int color)
    {
        // this function should return the input array with its outer edge painted with the given color
        // e.g. PaintBorder([[1, 2, 3], [4, 5, 6], [7, 8, 9]], 9) => [[9, 9, 9], [9, 5, 9], [9, 9, 9]]
        var xBorder = Matrix<float>.Build.DenseOfRowArrays(x.Select(row => row.Select(val => (float)val).ToArray()));
        for (var row = 0; row < xBorder.RowCount; row++)
        {
            for (var col = 0; col < xBorder.ColumnCount; col++)
            {
                if (row == 0 || row == xBorder.RowCount - 1 || col == 0 || col == xBorder.ColumnCount - 1)
                {
                    xBorder[row, col] = color;
                }
            }
        }
        return xBorder.ToColumnArrays().Select(row => row.Select(val => (int)val).ToArray()).ToArray();
    }
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

    public static IEnumerable<ObjectFeature> ExtractObjects(int[][] input)
    {
        IEnumerable<ObjectFeature> output = [];

        // a. Objectness priors:

        // Object cohesion: Ability to parse grids into “objects” based on continuity criteria including
        // color continuity or spatial contiguity (figure 5), ability to parse grids into zones, partitions.

        // Object persistence: Objects are assumed to persist despite the presence of noise (figure
        // 6) or occlusion by other objects. In many cases (but not all) objects from the input persist
        // on the output grid, often in a transformed form. Common geometric transformations of
        // objects are covered in category 4, “basic geometry and topology priors”.

        // Object influence via contact: Many tasks feature physical contact between objects (e.g.
        // one object being translated until it is in contact with another (figure 7), or a line “growing”
        // until it “rebounds” against another object (figure 8).

        // b. Goal-directedness prior: See ExtractGoalFeatures for more information.

        // c. Numbers and Counting priors:

        // Many ARC tasks involve counting or sorting objects (e.g. sorting by size), comparing
        // numbers (e.g. which shape or symbol appears the most (e.g. figure 10)? The least? The
        // same number of times? Which is the largest object? The smallest? Which objects are the
        // same size?), or repeating a pattern for a fixed number of time. The notions of addition and
        // subtraction are also featured (as they are part of the Core Knowledge number system as per
        // [85]). All quantities featured in ARC are smaller than approximately 10.

        // d. Basic Geometry and Topology priors:

        // ARC tasks feature a range of elementary geometry and topology concepts, in particular:
        // • Lines, rectangular shapes (regular shapes are more likely to appear than complex shapes).
        // • Symmetries (e.g. figure 11), rotations, translations.
        // • Shape upscaling or downscaling, elastic distortions.
        // • Containing / being contained / being inside or outside of a perimeter.
        // • Drawing lines, connecting points, orthogonal projections.
        // • Copying, repeating objects.

        return output;
    }

    public static IEnumerable<GoalFeature>? ExtractGoalFeatures(int[][] input, int[][] output)
    {

        // b. Goal-directedness prior:

        // While ARC does not feature the concept of time, many of the input/output grids can be
        // effectively modeled by humans as being the starting and end states of a process that in-
        // volves intentionality (e.g. figure 9). As such, the goal-directedness prior may not be strictly
        // necessary to solve ARC, but it is likely to be useful.
        throw new NotImplementedException();
    }


}
