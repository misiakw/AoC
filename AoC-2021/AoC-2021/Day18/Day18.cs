using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day18
{
    [BasePath("Day18")]
    [TestFile(File = "example.txt", Name = "Example")]
    //[TestFile(File = "Input.txt", Name = "Input")]
    public class Day18 : ParseableDay<SnailNumBase>
    {
        public Day18(string path) : base(path) { }

        public override SnailNumBase Parse(string input)
        {
            return SnailNumBase.Parse(input);
        }

        public override string Part1(string testName)
        {
            var c = SnailNumBase.Parse("[[6,[5,[4,[3,2]]]],1]");
            ((SnailNumPair)c).TestExplode(1);

            return c.ToString();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class SnailNumBase
    {
        public abstract long GetMagnitude();
        public static SnailNumBase Parse(string input)
        {
            var stack = new Stack<SnailNumBase>();
            SnailNumBase tmp = null;
            SnailNumRegular prev = null;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] >= '0' && input[i] <= '9')
                {
                    var str = "";
                    while (input[i] >= '0' && input[i] <= '9')
                    {
                        str = $"{str}{input[i]}";
                        i++;
                    }
                    var reg = new SnailNumRegular(long.Parse(str));
                    reg.Prev = prev;
                    if (prev != null) prev.Post = reg;
                    prev = reg;
                    tmp = reg;
                }
                if (input[i] == ',') stack.Push(tmp);
                if (input[i] == ']' || input[i] == '>')
                {
                    var left = stack.Pop();
                    var right = tmp;
                    tmp = new SnailNumPair(left, right);
                }
            }
            return tmp;
        }
        public static SnailNumBase operator +(SnailNumBase left, SnailNumBase right)
        {

            var result = new SnailNumPair(left, right);

            var pre = result.ToString();
            if (result.TestExplode(0))
            {

                var a = 5;
            }
            var post = result.ToString();
            return result;
        }
    }
    public class SnailNumPair : SnailNumBase
    {
        public bool Explode = false;
        private SnailNumBase Left, Right;
        public SnailNumPair(SnailNumBase left, SnailNumBase right)
        {
            this.Left = left;
            this.Right = right;
        }
        public override long GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();
        public bool TestExplode(int lvl)
        {
            if(lvl >3 && Left is SnailNumRegular && Right is SnailNumRegular)
            {
                if (((SnailNumRegular)Left).Prev != null)
                    ((SnailNumRegular)Left).Prev.Value += ((SnailNumRegular)Left).Value;

                if (((SnailNumRegular)Right).Post != null)
                    ((SnailNumRegular)Right).Post.Value += ((SnailNumRegular)Right).Value;
                Explode = true;
                return true;
            }
            var leftBool = false;
            var rightBool = false;
            if (Left is SnailNumPair)
            {
                leftBool = ((SnailNumPair)Left).TestExplode(lvl + 1);
                if (leftBool && ((SnailNumPair)Left).Explode) Left = new SnailNumRegular(0);
            }
            if (!leftBool && Right is SnailNumPair)
            {
                rightBool = ((SnailNumPair)Right).TestExplode(lvl + 1);
                if(rightBool && ((SnailNumPair)Right).Explode) Right = new SnailNumRegular(0);
            }

            return leftBool || rightBool;
        }
        public override string ToString() => $"[{Left},{Right}]";
    }
    public class SnailNumRegular : SnailNumBase
    {
        public SnailNumRegular Prev, Post;
        public long Value;
        public SnailNumRegular(long value)
        {
            this.Value = value;
        }

        public override long GetMagnitude() => Value;

        public override string ToString() => $"{Value}";
    }
}
