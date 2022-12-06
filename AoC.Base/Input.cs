namespace AoC.Base
{
    public class Input{
        public Input(string Path, string name){
            _filePath = Path;
            Name = name;

            if(!Directory.Exists(System.IO.Path.GetDirectoryName(_filePath))){
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_filePath).ToString());
            }
            if(!File.Exists(_filePath)){
                File.Create(_filePath).Close();
            }
        }
        public string Raw => File.ReadAllText(_filePath);
        public string[] Lines => Raw.Split("\n").Select(s => s.Trim()).ToArray();
        private readonly string _filePath;
        internal readonly TestType[] Tests = new TestType[2];
        public readonly Tuple<object, Type>[] Result = new Tuple<object, Type>[2];
        public readonly IList<object>[] Invalid = new IList<object>[2]{ new List<object>(), new List<object>()};
        public readonly string Name;
        private IList<object> _storedInput;
        public object[,] FailedResults = new object[2,2];
        public object Cache;

        public IList<string> Split(Func<string, IList<string>> splitFunc){
            return splitFunc(Raw.Trim()).Select(s => s.Trim()).ToList();
        }
        public IList<T> Prepare<T>(Func<string, IList<string>> splitFunc, Func<string, T> transformFunc, bool force=false){
            if (_storedInput == null || force == true){
                _storedInput = new List<object>();
                foreach(var line in Split(splitFunc))
                    if (!string.IsNullOrEmpty(line))
                        _storedInput.Add(transformFunc(line));
            }
            return _storedInput.Select(o => (T)o).ToList();
        }

        internal enum TestType{
            Skip = 0, 
            Verbal = 1, 
            Silent = 2
        }
    }
}
