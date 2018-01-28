using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Swappings
{
    internal class Swappings
    {
        private static void Main()
        {
            var max = int.Parse(Console.ReadLine());
            var splits = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var dict = new Dictionary<int, ChainLink<int>>();

            ChainLink<int> previous = null;
            ChainLink<int> first = null;
            ChainLink<int> last = null;

            for (int i = 0; i < max; i++)
            {
                var num = i + 1;
                
                var curr = new ChainLink<int>(num);

                if (num == 1)
                {
                    first = curr;
                }
                else
                {
                    curr.AttachPrevious(previous);
                }

                if (num == max)
                {
                    last = curr;
                }

                dict.Add(num, curr);
                previous = curr;
            }


            for (int i = 0; i < splits.Length; i++)
            {
                var splitNumber = splits[i];

                var middle = dict[splitNumber];
                ChainLink<int> prev;
                ChainLink<int> next;

                if (middle.Next == null)
                {
                    middle.AttachNext(first);
                    last = middle.SplitPrevious();
                    first = middle;
                    continue;
                }

                if (middle.Previous == null)
                {
                    middle.AttachPrevious(last);
                    first = middle.SplitNext();
                    last = middle;
                    continue;
                }

                prev = middle.SplitPrevious();
                next = middle.SplitNext();

                middle.AttachNext(first);
                middle.AttachPrevious(last);

                first = next;
                last = prev;
            }

            var sb = new StringBuilder();
            var node = first;

            while (node != null)
            {
                sb.Append($"{node.Value} ");
                node = node.Next;
            }

            Console.WriteLine(sb.ToString().Trim());
        }
    }

    internal class ChainLink<T>
    {
        private T value;
        public ChainLink(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get => this.value;
        }

        public ChainLink<T> Previous { get; private set; }

        public ChainLink<T> Next { get; private set; }

        public void AttachNext(ChainLink<T> link, bool isCalledByLink = false)
        {
            if (this.Next != null)
            {
                throw new InvalidOperationException("Already attached");
            }

            this.Next = link;

            if (isCalledByLink)
            {
                return;
            }
            if (link.Previous != null)
            {
                throw new InvalidOperationException("The other link already has an attachment");
            }

            link.AttachPrevious(this, true);
        }

        public void AttachPrevious(ChainLink<T> link, bool isCalledByLink = false)
        {
            if (this.Previous != null)
            {
                throw new InvalidOperationException("Already attached");
            }

            this.Previous = link;

            if (isCalledByLink)
            {
                return;
            }
            if (link.Next != null)
            {
                throw new InvalidOperationException("The other link already has an attachment");
            }
            link.AttachNext(this, true);
        }

        public ChainLink<T> SplitNext(bool isCalledByLink = false)
        {
            var next = this.Next;
            this.Next = null;

            if (!isCalledByLink)
            {
                next.SplitPrevious(true);
            }

            return next;
        }

        public ChainLink<T> SplitPrevious(bool isCalledByLink = false)
        {
            var prev = this.Previous;
            this.Previous = null;

            if (!isCalledByLink)
            {
                prev.SplitNext(true);
            }

            return prev;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}