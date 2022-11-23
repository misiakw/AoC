using System.Security.Cryptography;
using System.Text;

namespace AoC2016
{
    public class Day5 : DayBase
    {
        public Day5() : base(5)
        {
            Input("example1")
                .RunPart(1, "18f47a30")
                .RunPart(2, "05ace8e3")
            .Input("output")
                .RunPart(1, "2414bc77")
                .RunPart(2, "437e60fc");
        }

        public override object Part1(Input input)
        {
            var start = input.Raw.Trim();
            var counter = 0l;
            var result = string.Empty;
            var md5Results = new List<byte[]>();

            while (result.Length < 8)
            {
                var md5 = getMd5($"{start}{counter++}");
                if (md5[0] == 0 && md5[1] == 0 && md5[2] <= 0x0F)
                {
                    md5Results.Add(md5);
                    var hex = md5[2].ToString("x");
                    result += hex;
                }
            }
            input.Cache = Tuple.Create(counter, md5Results);

            return result;
        }

        public override object Part2(Input input)
        {
            var start = input.Raw.Trim();
            var result = new string[8];
            var precalculatedMd5 = ((Tuple<long, List<byte[]>>)input.Cache).Item2;
            var counter = ((Tuple<long, List<byte[]>>)input.Cache).Item1;
            foreach (var md5 in precalculatedMd5)
            {
                try
                {
                    var pos = int.Parse(md5[2].ToString("x"));
                    var num = md5[3].ToString("x2").Substring(0, 1);
                    if (pos < result.Length && string.IsNullOrEmpty(result[pos]))
                        result[pos] = num;
                }
                catch (FormatException e) { }
            }

            while (result.Any(s => string.IsNullOrEmpty(s)))
            {
                var md5 = getMd5($"{start}{counter++}");
                if (md5[0] == 0 && md5[1] == 0 && md5[2] <= 0x0F)
                {
                    try
                    {
                        var pos = int.Parse(md5[2].ToString("x"));
                        var num = md5[3].ToString("x2").Substring(0, 1);
                        if (pos < result.Length && string.IsNullOrEmpty(result[pos]))
                            result[pos] = num;
                    }
                    catch (FormatException e) { }
                }
            }

            return string.Join("", result);
        }

        private byte[] getMd5(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            using (MD5 md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(inputBytes);
                return hashBytes.Take(4).ToArray();
            }
        }
    }
}
