namespace AndyARC.Core;

using AndyARC.Core.Features;
using MathNet.Numerics.LinearAlgebra;
public static class SystemOne
{
    public static readonly int[] Colors = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
    public readonly static IEnumerable<Func<int[][], int[][]>> Actions =
    new List<Func<int[][], int[][]>> {
        Transpose,
        DeDupeXY,
        RotateSquares90,
        DoubleSize,
        Rotate90,
        ReflectAlongDiagonal,
        ReflectHorizontally,
        ReflectVertically,
        Rotate180,
        Rotate270,
        InvertColors
    }.Union(PaintBorders());

    private static IEnumerable<Func<int[][], int[][]>> PaintBorders()
    {
        return Colors.Select(color => (Func<int[][], int[][]>)(x => PaintBorder(x, color)));
    }
    public static int[][] PaintBorder(int[][] x, int color)
    {
        // this function should return the input array with its outer edge painted with the given color
        // e.g. PaintBorder([[1, 2, 3], [4, 5, 6], [7, 8, 9]], 9) => [[9, 9, 9], [9, 5, 9], [9, 9, 9]]
        var rowIter = 0;
        foreach (var row in x)
        {
            if (rowIter == 0 || rowIter == x.Length - 1)
            {
                for (var col = 0; col < row.Length; col++)
                {
                    row[col] = color;
                }
            }
            else
            {
                row[0] = color;
                row[^1] = color;
            }
            rowIter++;
        }
        return x;
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
    public static int[][] RotateSquares90(int[][] x)
    {
        if (!x.All(r => r.Length == x.Length))
        {
            return x;
        }
        // this function should return the input array rotated 90 degrees clockwise
        // e.g. Rotate90([[1, 2], [4, 5]]) => [[4, 1], [5, 2]]
        var xMatrix = Matrix<float>.Build.DenseOfRowArrays(x.Select(row => row.Select(val => (float)val).ToArray()));
        var xRot = xMatrix.ToColumnArrays().Select(row => row.Reverse().Select(val => (int)val).ToArray()).ToArray();
        return xRot;
    }
    public static int[][] Rotate90(int[][] x)
    {
        int numRows = x.Length;
        int numCols = x[0].Length;

        // Create a new array with swapped dimensions
        int[][] rotatedArray = new int[numCols][];
        for (int i = 0; i < numCols; i++)
        {
            rotatedArray[i] = new int[numRows];
        }

        // Iterate through each element in the input array
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // Place the element in the rotated position
                rotatedArray[j][numRows - 1 - i] = x[i][j];
            }
        }

        // Return the rotated array
        return rotatedArray;
    }
    public static IEnumerable<ObjectBox> ExtractObjects(int[][] input)
    {
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

        var objs = DetectObjects(input);

        // TODO: objs is a list of lists of coordinates representing every point in each object
        // return objs.Select(obj => new ObjectBox
        // {
        //     StartXY = (obj.Min(p => p.Item1), obj.Min(p => p.Item2)),
        //     EndXY = (obj.Max(p => p.Item1), obj.Max(p => p.Item2)),
        //     // RawData = obj.Select(p => input[p.Item1][p.Item2]).ToArray()
        // });

        return [];
    }
    public static IEnumerable<GoalFeature>? ExtractGoalFeatures(int[][] input, int[][] output)
    {
        // b. Goal-directedness prior:

        // While ARC does not feature the concept of time, many of the input/output grids can be
        // effectively modeled by humans as being the starting and end states of a process that in-
        // volves intentionality (e.g. figure 9). As such, the goal-directedness prior may not be strictly
        // necessary to solve ARC, but it is likely to be useful.

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
        return [];
    }
    public static int[][] ReflectAlongDiagonal(int[][] inputArray)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;

        // Create a new array with the same dimensions
        int[][] outputArray = new int[numCols][];
        for (int i = 0; i < numCols; i++)
        {
            outputArray[i] = new int[numRows];
        }

        // Iterate through each element in the input array
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // Reflect the element along the diagonal
                outputArray[j][i] = inputArray[i][j];
            }
        }

        // Return the transformed array
        return outputArray;
    }
    public static int[][] DoubleSize(int[][] inputArray)
    {
        // Get the dimensions of the input array
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;

        // Create a new array with the same dimensions
        int[][] outputArray = new int[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            outputArray[i] = new int[numCols];
        }

        // Iterate through each element in the input array
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // Double the value and store it in the output array
                outputArray[i][j] = inputArray[i][j] * 2;
            }
        }

        // Return the transformed array
        return outputArray;
    }
    public static int[][] ReflectHorizontally(int[][] inputArray)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;

        // Create a new array with the same dimensions
        int[][] outputArray = new int[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            outputArray[i] = new int[numCols];
        }

        // Iterate through each element in the input array
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // Reflect the element horizontally
                outputArray[i][numCols - 1 - j] = inputArray[i][j];
            }
        }

        // Return the transformed array
        return outputArray;
    }
    public static int[][] ReflectVertically(int[][] inputArray)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;

        int[][] outputArray = new int[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            outputArray[i] = new int[numCols];
        }

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                outputArray[numRows - 1 - i][j] = inputArray[i][j];
            }
        }

        return outputArray;
    }
    public static int[][] Rotate180(int[][] inputArray)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;

        int[][] outputArray = new int[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            outputArray[i] = new int[numCols];
        }

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                outputArray[numRows - 1 - i][numCols - 1 - j] = inputArray[i][j];
            }
        }

        return outputArray;
    }
    public static int[][] InvertColors(int[][] inputArray)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;
        int maxColor = Colors.Max();

        int[][] outputArray = new int[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            outputArray[i] = new int[numCols];
        }

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                outputArray[i][j] = maxColor - inputArray[i][j];
            }
        }

        return outputArray;
    }
    public static List<List<(int, int)>> DetectObjects(int[][] inputArray)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;
        bool[][] visited = new bool[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            visited[i] = new bool[numCols];
        }

        List<List<(int, int)>> objects = [];

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (!visited[i][j])
                {
                    List<(int, int)> obj = [];
                    FloodFill(inputArray, visited, i, j, inputArray[i][j], obj);
                    if (obj.Count > 0)
                    {
                        objects.Add(obj);
                    }
                }
            }
        }

        return objects;
    }
    private static void FloodFill(int[][] inputArray, bool[][] visited, int x, int y, int color, List<(int, int)> obj)
    {
        int numRows = inputArray.Length;
        int numCols = inputArray[0].Length;
        if (x < 0 || x >= numRows || y < 0 || y >= numCols || visited[x][y] || inputArray[x][y] != color)
        {
            return;
        }

        visited[x][y] = true;
        obj.Add((x, y));

        // Check all 4 directions (up, down, left, right)
        FloodFill(inputArray, visited, x - 1, y, color, obj);
        FloodFill(inputArray, visited, x + 1, y, color, obj);
        FloodFill(inputArray, visited, x, y - 1, color, obj);
        FloodFill(inputArray, visited, x, y + 1, color, obj);
    }
    public static int[][] Rotate270(int[][] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n / 2; i++)
        {
            int j = i + (n - i - 1);
            (arr[j], arr[i]) = (arr[i], arr[j]);
        }
        for (int i = 0; i < n / 2; i++)
        {
            int j = 2 * i + 1;
            int k = n - j;
            (arr[k], arr[j]) = (arr[j], arr[k]);
        }
        return arr;
    }
}
