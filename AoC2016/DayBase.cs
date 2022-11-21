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
        public DayBase RunPart(byte part) => RunPart<string>(part, null, false);
        public DayBase RunPart<T>(byte part, T result = default(T), bool rerun = false){
            if(part < 1 || part > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            tests.Last().Tests[part-1] = true;

            if(!EqualityComparer<T>.Default.Equals(result, default(T))){
                tests.Last().Result[part-1] = Tuple.Create((object?)result, typeof(T));
            }

            return this;
        }
        public abstract object Part1(Input input);
        public abstract object Part2(Input input);

        public virtual void Execute(){
            foreach(var test in tests.Where(t => t.Tests.All(b => b == null))){
                test.Tests[0] = true;
                test.Tests[1] = true;
            }
            
            ProcessTest(0, Part1);
            ProcessTest(1, Part2);
        }

        private void ProcessTest(byte testNum, Func<Input, object> testFunc){
            foreach(var test in tests.Where(t => t.Tests[testNum].HasValue && t.Tests[testNum].Value)){
                Console.WriteLine($"{test.Name} part {testNum+1}:");

                var resultObj = testFunc(test);
                var desiredResult = test.Result[testNum];
                
                if(resultObj != null){
                    if(desiredResult.Item1 == null){
                        Console.WriteLine($"Result[ {resultObj} ]");
                    }
                    else if(resultObj.GetType() != desiredResult.Item2){
                        Console.WriteLine($"Result[ {resultObj} ] Expected[ {desiredResult.Item1} ]");
                    }else{
                        var comparer = typeof(EqualityComparer<>)
                            .MakeGenericType(desiredResult.Item2)
                            .GetProperty("Default", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.Static)
                            .GetValue(null);
                            //.Equals(resultObj, desiredResult);
                        if(comparer.Equals(resultObj, desiredResult.Item1)){
                            Console.WriteLine$($"<< {resultObj} >>");
                        }else{
                            Console.WriteLine("dupa");
                        }
                    }
                }
                Console.WriteLine($"{resultObj} {resultObj.GetType()}");
            }
        }
    }

    public abstract class Day<T> : DayBase
    {
        public Day(int dayNum, bool forceInputParse) : base(dayNum){
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

    public class Input{
        public Input(string Path, string name){
            _filePath = Path;
            Name = name;
        }
        public string Raw{
            get{ return File.ReadAllText(_filePath);}
        }
        private readonly string _filePath;
        public readonly bool?[] Tests = new bool?[2];
        public readonly Tuple<object, Type>[] Result = new Tuple<object, Type>[2];
        public readonly string Name;
        private IList<object> _storedInput;

        public IList<string> Split(Func<string, IList<string>> splitFunc){
            return splitFunc(Raw.Trim()).Select(s => s.Trim()).ToList();
        }
        public IList<T> Prepare<T>(Func<string, IList<string>> splitFunc, Func<string, T> transformFunc, bool force=false){
            if (_storedInput == null || force == true){
                _storedInput = new List<object>();
                foreach(var line in Split(splitFunc))
                    _storedInput.Add(transformFunc(line));
            }
            return _storedInput.Select(o => (T)o).ToList();
        }
    }
}
