using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AoC2016
{
    public abstract class DayBase
    {
        private readonly int dayNum;
        protected IList<Input> tests = new List<Input>();

        public DayBase(int dayNum)
        {
            this.dayNum = dayNum;
        }

        public DayBase Input(string name)
        {
            var a = new Input($"./Inputs/Day{dayNum}/{name}.txt", name);
            tests.Add(a);
            return this;
        }

        public abstract string Part1(Input input);
        public abstract string Part2(Input input);

        public virtual void Execute(){
            foreach(var test in tests){
                Console.WriteLine($"<<Test {test.Name}>>\nPart 1:");
                Part1(test);
                Console.WriteLine("Part 2:");
                Part2(test);
            }
        }
    }

    public abstract class Day<T> : DayBase
    {
        public Day(int dayNum, bool forceInputParse) : base(dayNum){
            _forceInputParse = forceInputParse;
        }
        private bool _forceInputParse;
        public abstract string Part1(IList<T> data, Input input);
        public abstract string Part2(IList<T> data, Input input);
        public abstract IList<string> Split(string val);
        public abstract T Parse(string val);
        public override string Part1(Input input) => Part1(input.Prepare<T>(Split, Parse), input);

        public override string Part2(Input input) => Part2(input.Prepare<T>(Split, Parse), input);
    }

    public class Input{
        public Input(string Path, string name){
            _filePath = Path;
            Name = name;
        }
        public string Raw{
            get{ return File.ReadAllText(_filePath);}
        }
        private readonly string _filePath;
        public readonly string Name;
        private IList<object> _storedInput;

        public IList<string> Split(Func<string, IList<string>> splitFunc){
            return splitFunc(Raw.Trim()).Select(s => s.Trim()).ToList();
        }
        public IList<T> Prepare<T>(Func<string, IList<string>> splitFunc, Func<string, T> transformFunc, bool force=false){
            if (_storedInput == null || force == true){
                Console.WriteLine("process output");
                _storedInput = new List<object>();
                foreach(var line in Split(splitFunc))
                    _storedInput.Add(transformFunc(line));
            }
            return _storedInput.Select(o => (T)o).ToList();
        }
    }
}
