using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day8 : DayBase
    {
        public Day8() : base(8)
        {
            Input("example1")
                .RunPart(1, 21)
                .RunPart(2, 8)
            .Input("output")
                .RunPart(1, 1684)
                .RunPart(2, 486540);
        }

        public override object Part1(Input input)
        {
            var width = input.Lines.First().Count();
            var height = input.Lines.Count();
            var trees = ReadInput(input);

            trees = SetBordersVisible(trees, width, height);

            for(var y=1; y<height-1; y++){
                for(var x=1; x<width-1; x++){
                    trees[x, y].IsVisible = IsVisible(trees, x, y, width, height);
                }
            }
        
            //Print(trees, width, height);

            var count = 0;
            for(var x=0; x<width; x++)
                for(var y=0; y<height; y++)
                    if(trees[x,y].IsVisible)
                        count++;

            return count;
        }

        public override object Part2(Input input)
        {
            var width = input.Lines.First().Count();
            var height = input.Lines.Count();
            var scenicScore = 0;
            var trees = ReadInput(input);

            for(var y=1; y<height-1; y++){
                for(var x=1; x<width-1; x++){
                    var thisScore = GetScenicScore(trees, x, y, width, height);
                    if (thisScore > scenicScore) scenicScore = thisScore;
                }
            }

            return scenicScore;
        }

        private Tree[,] ReadInput(Input input){
            var width = input.Lines.First().Count();
            var height = input.Lines.Count();
            var trees = new Tree[width, height];

            var y = 0;
            foreach(var line in input.Lines){
                var x = 0;
                foreach (var ch in line){
                    trees[x, y] = new Tree((byte)(ch-'0'));
                    x++;
                }
                y++;
            }
            return trees;
        }

        private Tree[,] SetBordersVisible(Tree[,] trees, int width, int height){
            for(int x=0; x<width; x++){
                trees[x, 0].IsVisible = true;
                trees[x, height-1].IsVisible = true;
            }
            for(int y=0; y<height; y++){
                trees[0, y].IsVisible = true;
                trees[width-1, y].IsVisible = true;
            }
            return trees;
        }

        private bool IsVisible(Tree[,] trees, int x, int y, int width, int height){
            var left = new List<int>();
           
            for(var dx=x-1; dx>=0; dx--) left.Add(trees[dx, y].Height);
            if(left.All(h => h < trees[x, y].Height)) return true;

            var right = new List<int>();
            for(var dx=x+1; dx<width; dx++) right.Add(trees[dx, y].Height);
            if(right.All(h => h < trees[x, y].Height)) return true;

            var top = new List<int>();
            for(var dy=y-1; dy>=0; dy--) top.Add(trees[x, dy].Height);
            if(top.All(h => h < trees[x, y].Height)) return true;

            var bottom = new List<int>();
            for(var dy=y+1; dy<height; dy++) bottom.Add(trees[x, dy].Height);
            if(bottom.All(h => h < trees[x, y].Height)) return true;

            return false;
        }

        private int GetScenicScore(Tree[,] trees, int x, int y, int width, int height){
            var score = 1;

            var curr = 0;
            do{
                curr++;
            }while(x-curr > 0 && trees[x-curr, y].Height < trees[x,y].Height);
            score *= curr;

            curr = 0;
            do{
                curr++;
            }while(x+curr < width-1 && trees[x+curr, y].Height < trees[x,y].Height);
            score *= curr;

            curr = 0;
            do{
                curr++;
            }while(y-curr > 0 && trees[x, y-curr].Height < trees[x,y].Height);
            score *= curr;

            curr = 0;
            do{
                curr++;
            }while(y+curr < height-1 && trees[x, y+curr].Height < trees[x,y].Height);
            score *= curr;

            return score;
        }

        private void Print(Tree[,] trees, int width, int height){
            for(var y=0; y<height; y++){
                for(var x=0; x<width; x++)
                    Console.Write(trees[x, y].IsVisible? (char)(trees[x,y].Height+'0'): '*');
                Console.WriteLine();
            }
        }

        private class Tree{
            public readonly byte Height;
            public bool IsVisible = false;
            public Tree(byte height){
                this.Height = height;
            }
        }
    }
}
