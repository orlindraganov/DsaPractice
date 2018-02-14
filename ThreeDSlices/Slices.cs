using System;
using System.Linq;
using System.Text;

namespace ThreeDSlices
{
    class Slices
    {
        static void Main()
        {
            var sizes = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var width = sizes[0];
            var height = sizes[1];
            var depth = sizes[2];

            var totalSum = 0;

            var widthResults = new int[width];
            var heightResults = new int[height];
            var depthResults = new int[depth];

            for (int high = 0; high < height; high++)
            {
                var input = Console.ReadLine();
                var currentIndex = 0;

                for (int deep = 0; deep < depth; deep++)
                {
                    for (int sideways = 0; sideways < width; sideways++)
                    {
                        currentIndex = GetNumberIndex(currentIndex, input);
                        var length = GetNumberLength(currentIndex, input);

                        var number = int.Parse(input.Substring(currentIndex, length));

                        widthResults[sideways] += number;
                        heightResults[high] += number;
                        depthResults[deep] += number;

                        totalSum += number;

                        currentIndex += length;
                    }
                }
            }

            var possibilities = 0;

            var firstResult = widthResults[0];
            var secondResult = totalSum - firstResult;

            for (int i = 1; i < width; i++)
            {
                if (firstResult == secondResult)
                {
                    possibilities++;
                }

                firstResult += widthResults[i];
                secondResult -= widthResults[i];
            }

            firstResult = heightResults[0];
            secondResult = totalSum - firstResult;

            for (int i = 1; i < height; i++)
            {
                if (firstResult == secondResult)
                {
                    possibilities++;
                }

                firstResult += heightResults[i];
                secondResult -= heightResults[i];
            }

            firstResult = depthResults[0];
            secondResult = totalSum - firstResult;

            for (int i = 1; i < depth; i++)
            {
                if (firstResult == secondResult)
                {
                    possibilities++;
                }

                firstResult += depthResults[i];
                secondResult -= depthResults[i];
            }

            Console.WriteLine(possibilities);
        }

        private static int GetNumberIndex(int startingIndex, string s)
        {
            char curr = s[startingIndex];

            while (!char.IsDigit(curr))
            {
                startingIndex++;
                if (startingIndex > s.Length - 1)
                {
                    return -1;
                }
                curr = s[startingIndex];
            }

            return startingIndex;
        }

        private static int GetNumberLength(int startingIndex, string s)
        {
            var length = 0;

            var currentIndex = startingIndex + length;

            if (currentIndex > s.Length - 1)
            {
                return -1;
            }
            char curr = s[startingIndex + length];


            while (char.IsDigit(curr))
            {
                length++;
                currentIndex = startingIndex + length;
                if (currentIndex > s.Length - 1)
                {
                    length--;
                    break;
                }
                curr = s[startingIndex + length];
            }

            return length;
        }
    }
}
