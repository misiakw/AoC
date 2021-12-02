using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared;

namespace Day4
{
    public class Day4: IDay
    {
        public Conf Config => new Conf();

        public string Task1(IList<string> input)
        {
                var states = ParseInput(input);
                var lastAsleep = 0;
                var longestSleeping = 0l;
                var longestSleepingTime = 0;
                foreach (var guardGroup in states.GroupBy(s => s.GuardId))
                {
                    var sumOfSleep = 0;
                    foreach (var state in guardGroup)
                    {
                        if (state.Condition == Condition.Asleep)
                            lastAsleep = state.Time.Minute;
                        else
                            sumOfSleep += state.Time.Minute - lastAsleep;
                    }

                    if (sumOfSleep > longestSleepingTime)
                    {
                        longestSleepingTime = sumOfSleep;
                        longestSleeping = guardGroup.First().GuardId;
                    }
                }

                var hours = new Dictionary<int, int>();
                var startOfSleep = 0;
                foreach(var state in states.Where(s => s.GuardId == longestSleeping).OrderBy(s => s.Time))
                {                        
                    if (state.Condition == Condition.Asleep)
                        startOfSleep = state.Time.Minute;
                    else
                        for(var t = startOfSleep; t < state.Time.Minute; t++)
                        {
                            if (!hours.ContainsKey(t))
                                hours.Add(t, 0);
                            hours[t]++;
                        }
                }

                var hMax = hours.Max(v => v.Value);
                var timeMax = hours.First(h => h.Value == hours.Max(x => x.Value)).Key;

                return (timeMax*longestSleeping).ToString();
        }

        public string Task2(IList<string> input)
        {
            var states = ParseInput(input);

                var dict = new List<Tuple<long, int, int>>();
                foreach (var guard in states.GroupBy(s => s.GuardId))
                {
                    var hours = new Dictionary<int, int>();
                    var sleepStart = 0;
                    foreach (var state in guard)
                    {
                        if (state.Condition == Condition.Asleep)
                            sleepStart = state.Time.Minute;
                        else
                            for (var t = sleepStart; t < state.Time.Minute; t++)
                            {
                                if (!hours.ContainsKey(t))
                                    hours.Add(t, 0);
                                hours[t]++;
                            }
                    }

                    var maxCount = hours.Max(v => v.Value);
                    var maxHour = hours.First((x => x.Value == maxCount)).Key;
                    var guardId = guard.First().GuardId;

                    dict.Add(new Tuple<long, int, int>(guardId, maxHour, maxCount));
                }



                var maxList = dict.OrderByDescending(x => x.Item3);
                var max = maxList.First();

                return (max.Item1 * max.Item2).ToString();
        }

        private IList<State> ParseInput(IList<string> input)
        {
            var tmp = new List<Tuple<DateTime, string>>();
            foreach (var line in input)
            {
                var action = line.Substring(19);
                var timestamp = DateTime.Parse(line.Substring(1, 16));

                tmp.Add(new Tuple<DateTime, string>(timestamp, action));
            }

            var result = new List<State>();
            var guardId = 0;
            foreach(var state in tmp.OrderBy(x => x.Item1))
            { 
                if (state.Item2.ToLower().StartsWith("guard"))
                {
                    var match = new Regex("#(\\d*)").Match(state.Item2.ToLower());
                    guardId = int.Parse(match.Groups[1].Value);
                }
                else 
                {
                    result.Add(new State()
                    {
                        GuardId = guardId,
                        Time = state.Item1,
                        Condition = state.Item2.ToLower().StartsWith("falls")
                            ? Condition.Asleep
                            : Condition.Awake
                    });
                }
            }

            return result;
        }
    }

    public class State
    {
        public long GuardId { get; set; }
        public DateTime Time { get; set; }
        public Condition Condition { get; set; }

    }
    public enum Condition
    {
        Awake, Asleep
    }
}
