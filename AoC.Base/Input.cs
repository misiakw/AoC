using Path = System.IO.Path;
namespace AoC.Base
{
    public class Input
    {
        public Input(string FilePath, string name)
        {
            _filePath = FilePath;
            Name = name;

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? string.Empty);
            }
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
        }
        protected string _content = string.Empty;
        public string Raw
        {
            get
            {
                if (string.IsNullOrEmpty(_content))
                    _content = File.ReadAllText(_filePath).Replace("\r", "");
                return _content;
            }
        }
        public string[] Lines => Raw.Split("\n").Select(s => s.Trim()).ToArray();
        private readonly string _filePath;
        public string InputDir => System.IO.Path.GetDirectoryName(_filePath)?.ToString() ?? string.Empty;
        internal readonly TestType[] Tests = new TestType[2];
        public readonly Tuple<object, Type>[] Result = new Tuple<object, Type>[2];
        public readonly IList<object>[] Invalid = new IList<object>[2] { new List<object>(), new List<object>() };
        public readonly string Name;
        private IList<object?> _storedInput = new List<object?>();
        public object[,] FailedResults = new object[2, 2];
        public object? Cache = null;

        public IList<string> Split(Func<string, IList<string>> splitFunc)
        {
            return splitFunc(Raw.Trim()).Select(s => s.Trim()).ToList();
        }
        public IList<T> Prepare<T>(Func<string, IList<string>> splitFunc, Func<string, T> transformFunc, bool force = false)
        {
            if (!_storedInput.Any() || force == true)
            {
                _storedInput = new List<object?>();
                foreach (var line in Split(splitFunc))
                    if (!string.IsNullOrEmpty(line))
                        _storedInput.Add(transformFunc(line));
            }
            return (IList<T>)_storedInput.Select(o => (T?)o).ToList();
        }

        internal enum TestType
        {
            Skip = 0,
            Verbal = 1,
            Silent = 2
        }
    }
}
