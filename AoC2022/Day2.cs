using AoC.Base;

namespace AoC2022
{
    public class Day2 : Day<char[]>
    {
        public Day2() : base(2, false)
        {
            Input("example1")
                .RunPart(1, 15)
                .RunPart(2, 12)
            .Input("output")
                .RunPart(1, 10624)
                .RunPart(2, 14060);
        }

        public override char[] Parse(string val) => new char[2]{val[0], val[2]};

        public override object Part1(IList<char[]> data, Input input)
        {
            var score = 0;
            foreach(var set in data){
                var oponent = set[0] == 'A' ? Bet.Rock: set[0] == 'B' ? Bet.Paper: Bet.Sisors;
                var me = set[1] == 'X' ? Bet.Rock: set[1] == 'Y' ? Bet.Paper: Bet.Sisors;

                score += me == Bet.Rock ? 1: me == Bet.Paper ? 2: 3;

                if (oponent == me) score += 3;
                else if (IsWin(me, oponent)) score += 6;
            }

            return score;
        }

        public override object Part2(IList<char[]> data, Input input)
        {
            var score = 0;
            foreach(var set in data){
                var oponent = set[0] == 'A' ? Bet.Rock: set[0] == 'B' ? Bet.Paper: Bet.Sisors;
                Bet me = Bet.Paper;

                switch(set[1]){
                    case 'X': //loose
                        me = GetLoose(oponent);
                    break;
                    case 'Y': //draw
                        me = oponent;
                    break;
                    case 'Z': //win
                        me = GetWin(oponent);
                    break;
                }

                score += me == Bet.Rock ? 1: me == Bet.Paper ? 2: 3;

                if (oponent == me) score += 3;
                else if (IsWin(me, oponent)) score += 6;
            }

            return score;
        }

        public override IList<string> Split(string val) => val.Split("\n").ToList();

        private bool IsWin(Bet a, Bet b)
        {
            if (a == Bet.Rock && b == Bet.Sisors)
                return true;
            if (a == Bet.Paper && b == Bet.Rock)
                return true;
            if (a == Bet.Sisors && b == Bet.Paper)
                return true;
            return false;
        }

        private Bet GetLoose(Bet a) => a == Bet.Paper ? Bet.Rock : a == Bet.Rock ? Bet.Sisors : Bet.Paper;
        private Bet GetWin(Bet a) => a == Bet.Paper ? Bet.Sisors : a == Bet.Rock ? Bet.Paper : Bet.Rock;

        private enum Bet{
            Rock, 
            Paper, 
            Sisors
        }
    }
}
