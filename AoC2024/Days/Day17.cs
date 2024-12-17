using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AoCBase2;

namespace AoC2024.Days;

public class Day17: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day17>(17)
        .Callback(1, (d, t) => d.Part1(t.GetLines().ToArray()))
        //.Callback(2, (d, t) => d.Part2(t.GetLines()))
        .Test("debug", true).Skip()
        .Test("example")
        .Test("input")
        //.Part(1).Correct("1,7,2,1,4,1,5,4,0")
        //.Part(2).Correct()
        .Run();

    private int A, B, C;
    private int ptr = 0;
    
    public string Part1(string[] input)
    {
        var reg = new Regex($"Register .: (?<num>\\d+)");
        
        A = int.Parse(reg.Match(input[0]).Groups["num"].Value);
        B = int.Parse(reg.Match(input[1]).Groups["num"].Value);
        C = int.Parse(reg.Match(input[2]).Groups["num"].Value);

        var cmds = new Regex($"Program: (?<code>[\\d,]+)").Match(input[4]).Groups["code"]
            .Value.Split(',').Select(int.Parse).ToArray();

        IList<int> output = new List<int>();
        for (;ptr < cmds.Length;ptr += 2)
        {
            switch (cmds[ptr])
            {
                case 0: //adv
                    A = (int)((double)A / Math.Pow(2, Combo(cmds[ptr + 1])));
                    break;
                case 1: //bxl 
                    B = B ^ cmds[ptr + 1];
                    break;
                case 2: //bst <-- prepare to jump 
                    B = Combo(cmds[ptr + 1])%8;
                    break;
                case 3: //jnz <--- jump
                    if (A != 0) ptr = cmds[ptr + 1] - 2; // -2 to negate pointer increase
                    break;
                case 4: // bxc
                    B = B ^ C;
                    break;
                case 5: // out
                    output.Add(Combo(cmds[ptr + 1])%8);
                    break;
                case 6: //bdv
                    B = (int)((double)A / Math.Pow(2, Combo(cmds[ptr + 1])));
                    break;
                case 7: //cdv
                    C = (int)((double)A / Math.Pow(2, Combo(cmds[ptr + 1])));
                    break;
            }
        }
        
        return String.Join(",", output);
    }

    private int Combo(int val) => val switch
    {
        0 => 0,
        1 => 1,
        2 => 2,
        3 => 3,
        4 => A,
        5 => B,
        6 => C
    };
    
    //Part 2
    /*
     * Register A: 27575648
       Register B: 0
       Register C: 0
       
       Program: 2,4,1,2,7,5,4,1,1,3,5,5,0,3,3,0
       
       
       2,4 A = 27575648 B = 0 C = 0 			B = A % 4 
       1,2 A = 27575648 B = 2 C = 0			B = B ^ 2
       7,5 A = 27575648 B = 2 C = 6893912		C = A / (2^B)
       4,1 A = 27575648 B = 6893914 C = 6893912	B = B xor C
       1,3 A = 27575648 B = 6893913 C = 6893912	B = b xor 3
       5,5 Out 1					PRINT B%8 (last 3 bits)
       0,3 A = 9191882 B = 6893913 C = 6893912		A = A/3
       3,0 < loop to 0
       
       przedostatnia linia dzieli A na 3, a kończymy na 16 znakach, co ogranicza A
     * 
     */
}