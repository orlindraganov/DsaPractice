using System;
using System.Linq;

namespace ThreeDSlices
{
    class Slices
    {
        static void Main()
        {
            var sizes = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var cuboid = new int[sizes[0], sizes[1], sizes[2]];

            for (int i = 0; i < sizes[1]; i++)
            {
                var numbersAsString = Console.ReadLine().Split('|').Select(x => x.Trim()).ToArray();

                for (int j = 0; j < sizes[2]; j++)
                {
                    var numbers = numbersAsString[j].Split().Select(int.Parse).ToArray();

                    for (int k = 0; k < sizes[0]; k++)
                    {
                        cuboid[k, i, j] = numbers[k];
                    }
                }
            }

            var possibilities = 0;

            for (int j = 0; j < 3; j++)
            {
                for (int i = 1; i < cuboid.GetLength(j); i++)
                {
                    if (cuboid.SubCuboidSum(j, 0, i) == cuboid.SubCuboidSum(j, i))
                    {
                        possibilities++;
                    }
                }
            }

            Console.WriteLine(possibilities);
        }
    }

    static class CuboidExtensions
    {
        internal static int SubCuboidSum(this int[,,] cuboid, int dimension, int startingIndex)
        {
            if (dimension < 0 || 2 < dimension)
            {
                throw new ArgumentException("Dimension must be between 0 and 2!");
            }
            var length = cuboid.GetLength(dimension) - startingIndex;

            return cuboid.SubCuboidSum(dimension, startingIndex, length);
        }

        internal static int SubCuboidSum(this int[,,] cuboid, int dimension, int startingIndex, int sliceLength)
        {
            if (dimension < 0 || 2 < dimension)
            {
                throw new ArgumentException("Dimension must be between 0 and 2!");
            }

            int sliceDimension;
            int sliceRowDimension;
            int sliceColDimension;

            int cuboidZeroSize = cuboid.GetLength(0);
            int cuboidFirstSize = cuboid.GetLength(1);
            int cuboidSedcondSize = cuboid.GetLength(2);

            var sum = 0;

            if (dimension == 0)
            {
                sliceDimension = 0;
                sliceRowDimension = 1;
                sliceColDimension = 2;

                for (int i = 0; i < sliceLength; i++)
                {
                    var cuboidIndex = i + startingIndex;

                    for (int j = 0; j < cuboidFirstSize; j++)
                    {
                        for (int k = 0; k < cuboidSedcondSize; k++)
                        {
                            sum += cuboid[cuboidIndex, j, k];
                        }
                    }
                }
            }

            else if (dimension == 1)
            {
                sliceDimension = 1;
                sliceRowDimension = 2;
                sliceColDimension = 0;

                for (int i = 0; i < sliceLength; i++)
                {
                    var cuboidIndex = i + startingIndex;

                    for (int j = 0; j < cuboidSedcondSize; j++)
                    {
                        for (int k = 0; k < cuboidZeroSize; k++)
                        {
                            sum += cuboid[k, cuboidIndex, j];
                        }
                    }
                }
            }
            else
            {
                sliceDimension = 2;
                sliceRowDimension = 0;
                sliceColDimension = 1;

                for (int i = 0; i < sliceLength; i++)
                {
                    var cuboidIndex = i + startingIndex;

                    for (int j = 0; j < cuboidZeroSize; j++)
                    {
                        for (int k = 0; k < cuboidFirstSize; k++)
                        {
                            sum += cuboid[j, k, cuboidIndex];
                        }
                    }
                }
            }

            return sum;
        }

        internal static int Sum(this int[,,] cuboid)
        {
            var sum = 0;

            for (int i = 0; i < cuboid.GetLength(0); i++)
            {
                for (int j = 0; j < cuboid.GetLength(1); j++)
                {
                    for (int k = 0; k < cuboid.GetLength(2); k++)
                    {
                        sum += cuboid[i, j, k];
                    }
                }
            }

            return sum;
        }
    }
}
