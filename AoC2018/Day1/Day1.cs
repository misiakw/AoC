using System.Collections.Generic;
using Shared;

namespace Day1
{
    public class Program : IDay
    {
        public Conf Config => new Conf();

        public IList<string> ParseInput(IList<string> input)
        {
            return input;
        }

        public string Task1(IList<string> input)
        {
            var state = 0l;
            foreach (var line in input)
            {
                var change = long.Parse(line);
                state += change;
            }

            return state.ToString();

        }

        public string Task2(IList<string> input)
        {
            var states = new List<long>();
            var state = 0l;
            do
            {
                foreach (var line in input)
                {
                    var change = long.Parse(line);
                    state += change;
                    if (states.Contains(state))
                    {
                        return state.ToString();
                    }

                    states.Add(state);
                }
            } while (true);
        }

    }
}
