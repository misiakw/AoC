using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AoC2016
{
    public abstract partial class DayBase
    {
        private readonly int dayNum;
        protected IList<Input> tests = new List<Input>();

        public DayBase(int dayNum)
        {
            this.dayNum = dayNum;
        }
        public abstract object Part1(Input input);
        public abstract object Part2(Input input);

        public virtual void Execute(){           
            ProcessTest(0, Part1);
            ProcessTest(1, Part2);
        }

        private void ProcessTest(byte testNum, Func<Input, object> testFunc)
        {
            Console.WriteLine($"==== Part {testNum + 1} ====");
            foreach (var test in tests.Where(t => t.Tests[testNum] != AoC2016.Input.TestType.Skip))
            {
                try
                {
                    var resultObj = testFunc(test);

                    if (test.Tests[testNum] == AoC2016.Input.TestType.Silent)
                        continue;

                    Console.WriteLine($"\t{test.Name}");
                    var desiredResult = test.Result[testNum];

                    if (resultObj != null)
                    {
                        if (desiredResult?.Item1 == null)
                        {
                            Console.WriteLine($"Result[ {resultObj} ]");
                        }
                        else if (resultObj.GetType() != desiredResult.Item2)
                        {
                            Console.WriteLine($"Result[ {resultObj} ] Expected[ {desiredResult.Item1} ]");
                        }
                        else
                        {
                            if (resultObj.GetType().GetInterface("IComparable") != null)
                            {
                                VerifyComparable((IComparable)resultObj, desiredResult.Item1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exeption: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    continue;
                }
            }
        }

        private void VerifyComparable(IComparable compResult, object desired)
        {
            switch (compResult.CompareTo(desired))
            {
                case -1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ {compResult} ] too low Expected[ { desired} ]");
                    Console.ResetColor();
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ {compResult} ] too High Expected[ { desired} ]");
                    Console.ResetColor();
                    break;
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Correct[ {compResult} ]");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Invalid compare Result[ {compResult} ] <> Expected[ { desired} ]");
                    Console.ResetColor();
                    break;
            }
        }
    }

    public abstract class Day<T> : DayBase
    {
        public Day(int dayNum, bool forceInputParse = true) : base(dayNum){
            _forceInputParse = forceInputParse;
        }
        private bool _forceInputParse;
        public abstract object Part1(IList<T> data, Input input);
        public abstract object Part2(IList<T> data, Input input);
        public abstract IList<string> Split(string val);
        public abstract T Parse(string val);
        public override object Part1(Input input) => Part1(input.Prepare<T>(Split, Parse), input);

        public override object Part2(Input input) => Part2(input.Prepare<T>(Split, Parse), input);
    }
}
