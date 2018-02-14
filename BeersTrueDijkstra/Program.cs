using System;
using System.Collections.Generic;
using System.Linq;

namespace BeersTrueDijkstra
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = Console.ReadLine().Split().Select(int.Parse).ToList();

            var height = input[0];
            var width = input[1];
            var beersCount = input[2];

            var start = new NonBeer(0, 0);
            start.BestTime = 0;

            var finish = new NonBeer(height - 1, width - 1);

            var beers = new List<IElement>();

            for (int i = 0; i < beersCount; i++)
            {
                var address = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var beer = new Beer(address[0], address[1]);

                beers.Add(beer);
            }

            foreach (var element in beers)
            {
                start.Destinations.Add(element);

                foreach (var beer in beers)
                {
                    if (beer != element)
                    {
                        element.Destinations.Add(beer);
                    }
                }

                element.Destinations.Add(finish);
            }

            start.CalculateDestinations();

            beers = beers.OrderBy(x => x).ToList();
            var hasChangeOccured = true;

            while (hasChangeOccured)
            {
                hasChangeOccured = false;
                foreach (var element in beers)
                {
                    if (element.IsVisited)
                    {
                        continue;
                    }
                    element.CalculateDestinations();
                    element.IsVisited = true;
                    hasChangeOccured = true;
                }
            }

            Console.WriteLine(finish.BestTime);
        }
    }

    public interface IElement : IComparable<IElement>
    {
        bool IsVisited { get; set; }
        int Row { get; }
        int Col { get; }
        int BestTime { get; set; }
        IList<IElement> Destinations { get; }

        void CalculateDestinations();
    }

    public abstract class Element : IElement, IComparable<IElement>
    {
        protected Element(int row, int col)
        {
            this.IsVisited = false;
            this.Row = row;
            this.Col = col;
            this.BestTime = int.MaxValue;
            this.Destinations = new List<IElement>();
        }

        public bool IsVisited { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int BestTime { get; set; }
        public IList<IElement> Destinations { get; }

        public void CalculateDestinations()
        {
            foreach (var destination in this.Destinations)
            {
                var time = this.BestTime + this.CalculateTime(destination);

                if (time >= destination.BestTime)
                {
                    continue;
                }

                destination.BestTime = time;
                destination.IsVisited = false;
            }
        }

        protected int CalculateTime(IElement el)
        {
            return this.CalculateTime(el.Row, el.Col);
        }

        protected virtual int CalculateTime(int row, int col)
        {
            var dist = Math.Abs(this.Row - row) + Math.Abs(this.Col - col);
            return dist;
        }

        public int CompareTo(IElement other)
        {
            return this.BestTime.CompareTo(other.BestTime);
        }
    }

    public class Beer : Element, IElement, IComparable<IElement>
    {
        public Beer(int row, int col) : base(row, col)
        {
        }

        protected override int CalculateTime(int row, int col)
        {
            return base.CalculateTime(row, col) - 5;
        }
    }

    public class NonBeer : Element, IElement, IComparable<IElement>
    {
        public NonBeer(int row, int col) : base(row, col)
        {
        }
    }
}