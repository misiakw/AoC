using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day20
{
    [BasePath("Day20")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day20 : DayBase
    {
        private readonly string Key;
        private Image initialImage = new Image('.');
        private ImagePrinter printer;
        public Day20(string filePath) : base(filePath)
        {
            printer = new ImagePrinter(InputDir);
            this.Key = LineInput[0];

            for (var y = 2; y < LineInput.Count(); y++)
                for (var x = 0; x < LineInput[y].Length; x++)
                    if (LineInput[y][x] == '#')
                        initialImage[x, y - 2] = '#';

        }

        [ExpectedResult(TestName = "Example", Result = "35")]
        [ExpectedResult(TestName = "Input", Result = "5249")]
        public override string Part1(string testName)
        {
            var img = initialImage;
            var repeat = 2;
            var shouldInvert = Key[0] == '#';
            var lastInvert = shouldInvert;

            while (repeat-- > 0)
            {
                if (shouldInvert)
                {
                    img = img.DuplicateWithKey(Key, lastInvert);
                    lastInvert = !lastInvert;
                }
                else
                {
                    img = img.DuplicateWithKey(Key, lastInvert);
                }
            }

            return img.Size.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "3351")]
        //[ExpectedResult(TestName = "Input", Result = "5249")]
        public override string Part2(string testName)
        {
            var img = initialImage;
            var repeat = 50;
            var shouldInvert = Key[0] == '#';
            var lastInvert = shouldInvert;

            while (repeat-- > 0)
            {
                if (shouldInvert)
                {
                    img = img.DuplicateWithKey(Key, lastInvert);
                    lastInvert = !lastInvert;
                }
                else
                {
                    img = img.DuplicateWithKey(Key, lastInvert);
                }
            }

            printer.DrawImage((int)(img.Width*4), (int)(img.Height*4), testName, (c,i) => { img.DrawFunc(c, i); });

            return img.Size.ToString();
        }

        private class Image: Array2D<char>
        {
            public Image(char def) : base(def) { }

            public long Size => _data.Values.Where(v => v == '#').Count();

            public void DrawFunc(Graphics canvas, System.Drawing.Image image)
            {
                int multiplier = 4;
                canvas.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);
                for (var y = _minY; y <= _maxY; y++)
                {
                    for (var x = _minX; x <= _maxX; x++)
                    {
                        var px = x - _minX;
                        var py = y - _minY;
                        if (this[x, y] == '#')
                            canvas.FillRectangle(Brushes.Black, px * multiplier, py * multiplier, multiplier, multiplier); ;
                    }
                }
            }

            public Image DuplicateWithKey(string key, bool invert)
            {
                var result = new Image(invert? '#': '.');
                for(var y=_minY-1; y<=_maxY+1; y++)
                    for(var x= _minX-1; x<=_maxX+1; x++)
                    {
                        var pos = GetValForKey(x, y);
                        result[x, y] = key[pos];
                    }
                return result;
            }

            private int GetValForKey(long x, long y)
            {
                var str = "";
                str += this[x - 1, y - 1];
                str += this[x, y - 1];
                str += this[x + 1, y - 1];

                str += this[x - 1, y];
                str += this[x, y];
                str += this[x + 1, y];

                str += this[x - 1, y + 1];
                str += this[x, y + 1];
                str += this[x + 1, y + 1];

                str = str.Replace('#', '1').Replace('.', '0');

                return Convert.ToInt32(str, 2);
            }
        }
    }
}
