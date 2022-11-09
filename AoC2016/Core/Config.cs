using System;
using System.Linq;

namespace AoC2016.Core
{
    public interface IConfig{
        IConfig Input(string name);
        IConfig OnStep(byte step);
        IConfig WithResult<R>(R result);
    }

    public class ConfigBase<T>: IConfig{
        public ConfigBase(int dayNum){
            DayNum = dayNum;
        }
        public readonly int DayNum;
        public IConfig? currentConfig;
        public IList<Input> Inputs = new List<Input>();
        private Input currentInput;


        public IConfig Input(string name){
            var filePath = $"./Inputs/Day{DayNum}/{name}.txt";
            currentInput = new SimpleInput(name, filePath);
            Inputs.Add(currentInput);
            return this;
        }
        public IConfig OnStep(byte step){
            if (currentInput != null)
                currentInput.OnStep(step);
            return this;
        }
        public IConfig WithResult<R>(R result){
            if (currentInput != null)
                currentInput.WithResult(result);
            return this;
        }
    }

    public abstract class Input{        
        public Input(string name, string filePath){
            Name = name;
            FilePath = filePath;
        }
        public readonly string Name;
        public readonly string FilePath;
        
        public AbstractResult[] results = new AbstractResult[2];
        private AbstractResult? currentResult;

        public void WithResult<R>(R result){
            currentResult = new Result<R>(result);
        }
        public void OnStep(byte step){
            if (step >= 1 && step <= 2){
                results[step-1] =  currentResult ?? new EmptyResult();
                currentResult = null;
            }
        }
    }

    public class SimpleInput: Input{
        public SimpleInput(string name, string filePath): base(name, filePath){ }
    }

    public abstract class AbstractResult{
        public byte step;
        public Type? type = typeof(AbstractResult);
    }
    public class Result<R>: AbstractResult{
        public Result(R result){
            type = typeof(Result<R>);
            this.result = result;
        }
        public R result;
    }
    public class EmptyResult: AbstractResult{ }
}
    