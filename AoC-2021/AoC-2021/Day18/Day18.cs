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

        [ExpectedResult(TestName = "Example", Result = "4140")]
        //[ExpectedResult(TestName = "Input", Result = "25200")]
        public override string Part1(string testName)
        {
            var result = Input.First();

            foreach(var add in Input.Skip(1))
            {
                result += add;
                var a = 5;
            }

            return result.GetMagnitude().ToString();
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
            //ToDo: polączyć left i right w next i prev;

            var wasChange = true;
            while (wasChange)
            {
                wasChange = false;
                if (result.TestExplode(0))
                {
                    wasChange = true;
                    continue;
                }
                if (result.Split())
                {
                    wasChange = true;
                    continue;
                }
            }
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
            var leftJoinSide = (Left is SnailNumRegular) ? (SnailNumRegular)Left : ((SnailNumPair)Left).GetRighttmostRegular();
            var rightJoinSide = (Right is SnailNumRegular) ? (SnailNumRegular)Right : ((SnailNumPair)Right).GetLeftmostRegular();
            if (leftJoinSide != null) leftJoinSide.Post = rightJoinSide;
            if (rightJoinSide != null) rightJoinSide.Prev = leftJoinSide;
        }
        public override long GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();
        public bool TestExplode(int lvl)
        {
            if (lvl > 3 && Left is SnailNumRegular && Right is SnailNumRegular)
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
                if (leftBool && ((SnailNumPair)Left).Explode)
                {
                    var left = Left as SnailNumPair;
                    var zero = new SnailNumRegular(0);

                    if (((SnailNumRegular)left.Left).Prev != null)
                    {
                        zero.Prev = ((SnailNumRegular)left.Left).Prev;
                        ((SnailNumRegular)left.Left).Prev.Post = zero;
                    }
                    if (((SnailNumRegular)left.Right).Post != null)
                    {
                        zero.Post = ((SnailNumRegular)left.Right).Post;
                        ((SnailNumRegular)left.Right).Post.Prev = zero;
                    }

                    Left = zero;
                }
            }
            if (!leftBool && Right is SnailNumPair)
            {
                rightBool = ((SnailNumPair)Right).TestExplode(lvl + 1);
                if (rightBool && ((SnailNumPair)Right).Explode)
                {
                    var right = Right as SnailNumPair;
                    var zero = new SnailNumRegular(0);

                    if (((SnailNumRegular)right.Left).Prev != null)
                        zero.Prev = ((SnailNumRegular)right.Left).Prev;
                    if (((SnailNumRegular)right.Right).Post != null)
                        zero.Post = ((SnailNumRegular)right.Right).Post;

                    Right = zero;
                }
            }

            return leftBool || rightBool;
        }

        private SnailNumRegular GetLeftmostRegular()
        {
            if (Left is SnailNumRegular)
                return Left as SnailNumRegular;
            else
                return ((SnailNumPair)Left).GetLeftmostRegular();
        }
        private SnailNumRegular GetRighttmostRegular()
        {
            if (Right is SnailNumRegular)
                return Right as SnailNumRegular;
            else
                return ((SnailNumPair)Right).GetRighttmostRegular();
        }

        public bool Split()
        {
            if (Left is SnailNumRegular)
            {
                if (TestSplitSide(ref Left))
                    return true;
            }
            else
                if (((SnailNumPair)Left).Split())
                return true;

            if (Right is SnailNumRegular)
            {
                if (TestSplitSide(ref Right))
                    return true;
            }
            else
                if (((SnailNumPair)Right).Split())
                return true;

            return false;
        }

        private bool TestSplitSide(ref SnailNumBase checkedNum)
        {
            var num = checkedNum as SnailNumRegular;
            if (num.Value > 9)
            {
                var newLeft = new SnailNumRegular(num.Value / 2);
                var newRight = new SnailNumRegular(num.Value - newLeft.Value);

                newLeft.Post = newRight;
                newLeft.Prev = num.Prev;
                if (num.Prev != null) num.Prev.Post = newLeft;

                newRight.Prev = newLeft;
                newRight.Post = num.Post;
                if (num.Post != null) num.Post.Prev = newRight;

                checkedNum = new SnailNumPair(newLeft, newRight);
                return true;
            }
            return false;
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
