using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCBase2.InputClasses
{
    public class LinesInput
    {
        public async IAsyncEnumerable<string> GetLines(string filepath)
        {
            using (StreamReader sr = File.OpenText(filepath))
                while (!sr.EndOfStream)
                    yield return await sr.ReadLineAsync();
        }
    }
}
