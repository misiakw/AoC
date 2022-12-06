using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace AoC.Common
{
    public static class Extensions
    {
        public static IList<Color> GetColors(this Color c, int amount)
        {
            var colorList = new List<Color>();

            if (amount < 5 * 5 * 5)
            {
                int dif = 255 / (amount / 3);
                var R = 0;
                var G = 0;
                var B = 0;

                for (var i = 0; i < amount; i += 7)
                {
                    colorList.Add(Color.FromArgb(255, R, G, B + dif));
                    colorList.Add(Color.FromArgb(255, R, G + dif, B));
                    colorList.Add(Color.FromArgb(255, R + dif, G, B));
                    colorList.Add(Color.FromArgb(255, R, G + dif, B + dif));
                    colorList.Add(Color.FromArgb(255, R + dif, G, B + dif));
                    colorList.Add(Color.FromArgb(255, R + dif, G + dif, B));
                    colorList.Add(Color.FromArgb(255, R + dif, G + dif, B + dif));
                }
                colorList = colorList.Take(amount).ToList();
            }
            else
            {
                var inc = (255 * 255 * 255) / (amount + 2);
                for (var i = 1; i <= amount; i++)
                    colorList.Add(Color.FromArgb(255, Color.FromArgb(i * inc)));
            }
                        
            colorList = colorList.Randomize().ToList();
            colorList.Insert(0, Color.Black);
            colorList.Insert(colorList.Count(), Color.White);

            return colorList.ToList();
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            return source.OrderBy(x => rand.Next(int.MinValue, int.MaxValue));
        }
    }
}
