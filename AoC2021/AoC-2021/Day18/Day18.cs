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
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day18 : DayBase
    {
        public Day18(string path) : base(path) { }


        [ExpectedResult(TestName = "Example", Result = "4140")]
        [ExpectedResult(TestName = "Input", Result = "3665")]
        public override string Part1(string testName)
        {
            var p1 = GetInput();
            var result = p1.First();
            foreach(var add in p1.Skip(1))
            {
                result += add;
                var d = 5;
            }

            return result.GetMagnitude().ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "3993")]
        [ExpectedResult(TestName = "Input", Result = "4775")]
        public override string Part2(string testName)
        {
            var results = new List<long>();

            foreach(var l in LineInput)
                foreach (var p in LineInput)
                {
                    if (l.Equals(p)) continue;
                    var r = SnailNumBase.Parse(l) + SnailNumBase.Parse(p);
                    results.Add(r.GetMagnitude());
                }
            return results.Max().ToString();
        }

        private IList<SnailNumBase> GetInput()
        {
            var result = new List<SnailNumBase>();
            foreach(var line in LineInput)
                result.Add(SnailNumBase.Parse(line));
            return result;
        }
    }
    public abstract class SnailNumBase
    {
        public abstract long GetMagnitude();
        public static SnailNumBase Parse(string input)
        {
            var stack = new Stack<SnailNumBase>();
            SnailNumBase tmp = null;
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

            var wasChange = true;
            while (wasChange)
            {
                wasChange = false;
                if (result.Explode(0))
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
        private SnailNumBase Left, Right;
        public SnailNumPair Root;
        public SnailNumPair(SnailNumBase left, SnailNumBase right)
        {
            this.Left = left;
            if (left is SnailNumPair) ((SnailNumPair)left).Root = this;
            this.Right = right;
            if (right is SnailNumPair) ((SnailNumPair)right).Root = this;
        }
        public override long GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();
        public bool Explode(int lvl)
        {
            if (lvl > 3 && Left is SnailNumRegular && Right is SnailNumRegular)
            {
                if (LeftLeaf != null) LeftLeaf.Value += ((SnailNumRegular)Left).Value;
                if (RightLeaf != null) RightLeaf.Value += ((SnailNumRegular)Right).Value;
                if (Root.Left == this) Root.Left = new SnailNumRegular(0);
                if (Root.Right == this) Root.Right = new SnailNumRegular(0);
                return true;
            }

            if (Left is SnailNumPair)
                if (((SnailNumPair)Left).Explode(lvl + 1))
                    return true;
            if (Right is SnailNumPair)
                if (((SnailNumPair)Right).Explode(lvl + 1))
                    return true;
            return false;
        }
        public bool Split()
        {
            if (VerifySplit(Left))
                return true;
            if (VerifySplit(Right))
                return true;
            return false;
        }

        private bool VerifySplit(SnailNumBase node)
        {
            if (node is SnailNumRegular)
            {
                var regNode = node as SnailNumRegular;
                if (regNode.Value >= 10)
                {
                    var leftNode = new SnailNumRegular(regNode.Value / 2);
                    var rightNode = new SnailNumRegular(regNode.Value - leftNode.Value);
                    var replaceNode = new SnailNumPair(leftNode, rightNode);
                    replaceNode.Root = this;
                    if (node == Left) Left = replaceNode;
                    else Right = replaceNode;
                    return true;
                }
            }
            else if (((SnailNumPair)node).Split())
            {
                return true;
            }
            return false;
        }

        protected SnailNumRegular LeftLeaf
        {
            get
            {
                if (Root == null) return null;
                if (Root.Right == this)
                {
                    var search = Root.Left;
                    while(search is SnailNumPair)
                    {
                        search = ((SnailNumPair)search).Right;
                    }
                    return (SnailNumRegular)search;
                }
                else
                {
                    return Root.LeftLeaf;
                }
            }
        }

        protected SnailNumRegular RightLeaf
        {
            get
            {
                if (Root == null) return null;
                if(Root.Left == this)
                {
                    var search = Root.Right;
                    while(search is SnailNumPair)
                    {
                        search = ((SnailNumPair)search).Left;
                    }
                    return (SnailNumRegular)search;
                }
                else
                {
                    return Root.RightLeaf;
                }
            }
        }

        public override string ToString() => $"[{Left},{Right}]";
    }
    public class SnailNumRegular : SnailNumBase
    {
        public long Value;
        public SnailNumRegular(long value)
        {
            this.Value = value;
        }

        public override long GetMagnitude() => Value;

        public override string ToString() => $"{Value}";
    }
}
