namespace AndyARC.Core
{
    public static class Knowledge
    {
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

        public static int[,] Transpose(int[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            int[,] result = new int[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }
    }
}