using System;

namespace AoC2016.Core
{
    public class Input<T>
    {
        private readonly string _filePath;
        public readonly string FileName;
        public Func<string[], string> splitFunc = (s => return new List<string>{s};);
        public Input(string dirPath, string filePath){
            if(!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
                
            FileName = filePath;
            _filePath = $"{dirPath}/{filePath}.txt";
            if(!File.Exists(_filePath)){
                File.Create(_filePath);
            }
        }

        public string ReadStr(){
            var contents = File.ReadAllText(_filePath).Trim();
            return contents;
        }

        public IEnumerable<string> Read(){
            return splitFunc.Invoke(ReadStr())).ToList();
        }
    }
}