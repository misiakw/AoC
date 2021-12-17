using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AoC_2021.Day16.Day16;

namespace AoC_2021.Day16
{
    [BasePath("Day16")]
    [TestFile(File = "SinglePacket.txt", Name = "SinglePacket", TestToProceed = TestCase.Part1)]
    [TestFile(File = "TwoSupackets.txt", Name = "TwoSupackets", TestToProceed = TestCase.Part1)]
    [TestFile(File = "example1.txt", Name = "Example1", TestToProceed = TestCase.Part1)]
    [TestFile(File = "example2.txt", Name = "Example2", TestToProceed = TestCase.Part1)]
    [TestFile(File = "example3.txt", Name = "Example3", TestToProceed = TestCase.Part1)]
    [TestFile(File = "example4.txt", Name = "Example4", TestToProceed = TestCase.Part1)]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day16 : ParseableDay<PacketBase>
    {
        public Day16(string filePath) : base(filePath)
        {
        }

        public override PacketBase Parse(string input)
        {
            return PacketBase.GetPacket(string.Join("", input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')).ToArray()));      
        }

        [ExpectedResult(TestName = "Example1", Result = "16")]
        [ExpectedResult(TestName = "Example2", Result = "12")]
        [ExpectedResult(TestName = "Example3", Result = "23")]
        [ExpectedResult(TestName = "Example4", Result = "31")]
        [ExpectedResult(TestName = "Input", Result = "852")]
        public override string Part1(string testName)
        {
            return Input[0].GetVersionSum().ToString();
        }

        [ExpectedResult(TestName = "Input", Result = "19348959966392")]
        public override string Part2(string testName)
        {
            return Input[0].GetValue().ToString(); ;
        }

        public abstract class PacketBase
        {
            public int Length { get; protected set; }
            public readonly int Version;
            public readonly int TypeId;
            public PacketBase(int version, int typeid, string payload)
            {
                Version = version;
                TypeId = typeid;
            }
            public static PacketBase GetPacket(string binaries)
            {
                var version = Convert.ToInt32(binaries.Substring(0, 3), 2);
                var typeId = Convert.ToInt32(binaries.Substring(3, 3), 2);
                binaries = binaries.Substring(6);

                switch (typeId)
                {
                    case 0: return new SumPacket(version, typeId, binaries);
                    case 1: return new ProductPacket(version, typeId, binaries);
                    case 2: return new MinimumPacket(version, typeId, binaries);
                    case 3: return new MaximumPacket(version, typeId, binaries);
                    case 4: return new LiteralPacket(version, typeId, binaries);
                    case 5: return new GraterPacket(version, typeId, binaries);
                    case 6: return new LessPacket(version, typeId, binaries);
                    case 7: return new EqualPacket(version, typeId, binaries);
                }

                return null;
            }

            public abstract int GetVersionSum();
            public abstract long GetValue();
        }

        public class LiteralPacket : PacketBase
        {
            private readonly long Value;
            public LiteralPacket(int version, int typeid, string payload) : base(version, typeid, payload)
            {
                var i = 0;
                string str = "";
                char firstChar;
                do
                {
                    firstChar = payload[i];
                    str = str+payload.Substring(i+1, 4);
                    i += 5;
                } while (firstChar == '1');
                Value += Convert.ToInt64(str, 2);
                Length = i+6;
            }

            public override long GetValue()
            {
                return Value;
            }

            public override int GetVersionSum()
            {
                return Version;
            }
        }
        public abstract class OperatorPacket : PacketBase
        {
            protected IList<PacketBase> innerPackets = new List<PacketBase>();
            public OperatorPacket(int version, int typeid, string payload) : base(version, typeid, payload)
            {
                if (payload[0] == '0')
                    LoadByBitLength(payload.Substring(1));
                else
                    LoadByPacketCount(payload.Substring(1));
            }

            private void LoadByBitLength(string payload)
            {//15
                var payloadLength = Convert.ToInt32(payload.Substring(0, 15), 2);
                payload = payload.Substring(15, payloadLength);
                Length = 7+15;

                while(payload.Length > 6)
                {
                    var packet = GetPacket(payload);
                    Length += packet.Length;
                    payload = payload.Substring(packet.Length);
                    innerPackets.Add(packet);
                }
            }
            private void LoadByPacketCount(string payload)
            {//11
                var packetsCount = Convert.ToInt32(payload.Substring(0, 11), 2);
                payload = payload.Substring(11);
                Length = 7+11;

                while(packetsCount > 0)
                {
                    var packet = GetPacket(payload);
                    Length += packet.Length;
                    payload = payload.Substring(packet.Length);
                    innerPackets.Add(packet);
                    packetsCount -= 1;
                }
            }

            public override int GetVersionSum()
            {
                return Version + innerPackets.Select(p => p.GetVersionSum()).Sum();
            }
        }

        public class SumPacket : OperatorPacket
        {
            public SumPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                return innerPackets.Select(p => p.GetValue()).Sum();
            }
        }
        public class ProductPacket : OperatorPacket
        {
            public ProductPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                var result = 1L;
                foreach (var packet in innerPackets.Select(p => p.GetValue()))
                    result *= packet;
                return result;
            }
        }
        public class MinimumPacket : OperatorPacket
        {
            public MinimumPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                return innerPackets.Select(p => p.GetValue()).Min();
            }
        }
        public class MaximumPacket : OperatorPacket
        {
            public MaximumPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                return innerPackets.Select(p => p.GetValue()).Max();
            }
        }
        public class GraterPacket : OperatorPacket
        {
            public GraterPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                return innerPackets[0].GetValue() > innerPackets[1].GetValue()
                    ? 1 : 0;
            }
        }
        public class LessPacket : OperatorPacket
        {
            public LessPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                return innerPackets[0].GetValue() < innerPackets[1].GetValue()
                    ? 1 : 0;
            }
        }
        public class EqualPacket : OperatorPacket
        {
            public EqualPacket(int version, int typeid, string payload) : base(version, typeid, payload) { }
            public override long GetValue()
            {
                return innerPackets[0].GetValue() == innerPackets[1].GetValue()
                    ? 1 : 0;
            }
        }
    }
}
