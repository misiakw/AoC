using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day8
{
    [Day("Day8")]
    [Input("Test", typeof(Day8TestInput))]
    [Input("AoC", typeof(Dat8AocInput))]
    public class Day8 : IDay
    {
        private IList<int[,]> _layers;
        private int _width;
        private int _height;
        public string Task1(IInput input)
        {
            var firstCut = input.Input.Split(";");
            var sizes = firstCut[0].Split("x").Select(int.Parse).ToArray();

            _width = sizes[0];
            _height = sizes[1];


            var currentLayer = new int[_width, _height];
            _layers = new List<int[,]>
            {
                currentLayer
            };


            var x = 0;
            var y = 0;
            foreach (char c in firstCut[1])
            {
                if (x == _width)
                {
                    x = 0;
                    y++;
                    if (y == _height)
                    {
                        currentLayer = new int[_width, _height];
                        _layers.Add(currentLayer);
                        y = 0;
                    }
                }
                currentLayer[x, y] = int.Parse(c.ToString());
                x++;
            }

            int[,] minLayer = null;
            int minZeros = 0;
            int thisOnes = 0;
            int thisTwoes = 0;
            foreach (var layer in _layers) {
                var zeros = 0;
                var ones = 0;
                var twoes = 0;
                for (x = 0; x < _width; x++)
                    for (y = 0; y < _height; y++)
                    {
                        if (layer[x, y] == 0)
                            zeros++;
                        if (layer[x, y] == 1)
                            ones++;
                        if (layer[x, y] == 2)
                            twoes++;
                    }

                if (minLayer == null || minZeros > zeros)
                {
                    minLayer = layer;
                    minZeros = zeros;
                    thisOnes = ones;
                    thisTwoes = twoes;
                }
            }
            return (thisOnes * thisTwoes).ToString();
        }

        public string Task2(IInput input)
        {
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    foreach (var layer in _layers)
                        if (layer[x, y] != 2)
                        {
                            Console.Write(layer[x, y] == 0 ? " " : "#");
                            break;
                        }
                }
                Console.WriteLine();
            }

            return "";
        }
    }
}
