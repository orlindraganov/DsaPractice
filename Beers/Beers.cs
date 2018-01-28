using System;
using System.Collections.Generic;
using System.Linq;

namespace Beers
{
    internal class Beers
    {
        private static void Main()
        {
            //
            // OOPлин proudly presents his rendition of Dijkstra's Algorithm.
            // 
            // Based on memories from Viktor's Lecture and the nice gif on wikipedia. 
            //
            // Note that there's no matrix. Nice, a?
            //
            
            var input = Console.ReadLine().Split().Select(int.Parse).ToList();

            var height = input[0];
            var width = input[1];
            var beersCount = input[2];

            var start = new Element(0, 0);
            var finish = new Element(height - 1, width - 1);

            finish.BestTime = finish.CalculateTime(start);
            var beers = new List<Element>();

            for (int i = 0; i < beersCount; i++)
            {
                var beer = Element.Parse(Console.ReadLine());
                beer.BestTime = beer.CalculateTime(start);
                beers.Add(beer);
            }

            beers = beers.OrderBy(x => x).ToList();

            // TODO: refactor the Element class so the innermost foreach goes to beer.ConnectedBeerPeers z.b.
            // Not needed in this implementation, where all the elements are interconnected but useful the future.
            // Also the algorithm ought to be improveable by not going to visited otherBeer. 

            var hasChangeOccured = true;
            while (hasChangeOccured)
            {
                hasChangeOccured = false;
                foreach (var beer in beers)
                {
                    if (beer.IsVisited)
                    {
                        continue;
                    }

                    foreach (var otherBeer in beers)
                    {
                        if (beer == otherBeer)
                        {
                            continue;
                        }

                        var time = beer.BestTime + beer.CalculateTime(otherBeer) - 5;

                        if (time >= otherBeer.BestTime)
                        {
                            continue;
                        }

                        if (time < 0)
                        {
                            time = 0;
                        }

                        otherBeer.BestTime = time;
                        otherBeer.IsVisited = false;
                        hasChangeOccured = true;
                    }

                    beer.IsVisited = true;
                }
            }

            foreach (var beer in beers)
            {
                var time = beer.BestTime + beer.CalculateTime(finish) - 5;

                if (time < 0)
                {
                    time = 0;
                }

                if (time < finish.BestTime)
                {
                    finish.BestTime = time;
                }
            }

            Console.WriteLine(finish.BestTime);
        }
    }

    internal interface IElement
    {
        bool IsVisited { get; set; }
        int Row { get; }
        int Col { get; }
        int BestTime { get; set; }

        int CalculateTime(IElement el);
    }

    internal class Element : IElement, IComparable<IElement>
    {
        public Element(int row, int col)
        {
            this.IsVisited = false;
            this.Row = row;
            this.Col = col;
        }

        public bool IsVisited { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int BestTime { get; set; }

        public static Element Parse(string s)
        {
            var pos = s.Split().Select(int.Parse).ToArray();

            return new Element(pos[0], pos[1]);
        }

        public int CalculateTime(IElement el)
        {
            return this.CalculateTime(el.Row, el.Col);
        }

        private int CalculateTime(int row, int col)
        {
            var dist = Math.Abs(this.Row - row) + Math.Abs(this.Col - col);
            return dist;
        }

        public int CompareTo(IElement other)
        {
            return this.BestTime.CompareTo(other.BestTime);
        }
    }
}
