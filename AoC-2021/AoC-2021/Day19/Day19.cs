using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day19
{
    [BasePath("Day19")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day19 : DayBase
    {
        private IReadOnlyDictionary<long, IList<Beacon>> Input;
        public Day19(string filePath) : base(filePath)
        {
            var input = LineInput.GetEnumerator();
            var dict = new Dictionary<long, IList<Beacon>>();

            while (input.MoveNext())
            {
                var key = long.Parse(input.Current.Replace("scanner", "").Replace("---", "").Trim());
                string line;
                var beacons = new List<Beacon>();

                while(input.MoveNext() && (line = input.Current.Trim()) != string.Empty)
                    beacons.Add(new Beacon(line.Split(",").Select(l => long.Parse(l)).ToArray()));

                foreach(var beacon in beacons)
                    foreach (var other in beacons.Where(b => b != beacon))
                        beacon.AddRelated(other);

                dict.Add(key, beacons);
            }
            Input = dict;
        }

        [ExpectedResult(TestName = "Example", Result = "79")]
        //[ExpectedResult(TestName = "Input", Result = 571032)]
        public override string Part1(string testName)
        {

            var match = GetMatching(Input[0], Input[1]).ToList(); ;

            for (var c = 0; c<Input.Count()-1; c++)
            {
                var current = Input[c];
                for(var m=c+1; m<Input.Count(); m++)
                {
                    var matching = GetMatching(current, Input[m]);
                    Console.WriteLine($"match {c} with {m}. Matches {matching.Count()}");
                    if (matching.Count() >= 12)
                    {
                        Console.WriteLine("Got match!");
                    }
                }
            }

            throw new NotImplementedException();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private IList<Tuple<Beacon, Beacon, Rotation3D>> GetMatching(IList<Beacon> current, IList<Beacon> match)
        {
            var unoriented = GetVectorMatch(current, match).ToList();
            //multiple rotations can be return, limit results to most facing the same way


            return unoriented.GroupBy(u => u.Item3).Where(g => g.Count() >= 120).FirstOrDefault()?.ToList();
        }

        private IEnumerable<Tuple<Beacon, Beacon, Rotation3D>> GetVectorMatch(IList<Beacon> current, IList<Beacon> match)
        {
            foreach(var beacon in current)
            {
                var potential = new List<Beacon>();
                foreach (var pretender in match)
                {
                    var rotation = beacon.GetRotation(pretender);
                    if (rotation != null)
                        yield return Tuple.Create(beacon, pretender, rotation);
                }
            }
        }

        private class Beacon: Vector3D
        {
            private IList<BeaconRelation> Relations = new List<BeaconRelation>();
            public IList<Beacon> OtherInstances = new List<Beacon>();
            public Beacon(long[] pos): base(pos[0], pos[1], pos[2])
            {
            }

            public IEnumerable<long> dif(Beacon beacon)
            {
                yield return X - beacon.X;
                yield return Y - beacon.Y;
                yield return Z - beacon.Z;
            }

            public void AddRelated(Beacon beacon)
            {
                var diff = dif(beacon).ToArray();
                var abs = diff.Select(d => Math.Abs(d)).ToArray();
                var moveVect = new Vector3D(diff[0], diff[1], diff[2]);

                var positions = new List<Vector3D>();
                var next = moveVect;
                //get all rotation twists across Z rotation
                for(var zRot=0; zRot < 4; zRot++) {
                    next = next.RotateCW(Axis.Z);
                    for (var rotX=0; rotX < 4; rotX++){
                        next = next.RotateCW(Axis.X);
                        positions.Add(next);
                    }
                }
                //get twist across Y rotation 90 deg
                next = next.RotateCW(Axis.Y);
                for (var rotZ = 0; rotZ < 4; rotZ++)
                {
                    next = next.RotateCW(Axis.Z);
                    positions.Add(next);
                }
                //get twist across Y rotation 270 deg
                next = next.RotateCW(Axis.Y).RotateCW(Axis.Y);
                for (var rotZ = 0; rotZ < 4; rotZ++)
                {
                    next = next.RotateCW(Axis.Z);
                    positions.Add(next);
                }
                Relations.Add(new BeaconRelation
                {
                    MoveVect = moveVect,
                    DistHash = string.Join("|", abs.OrderBy(x => x)),
                    Target = beacon,
                    Positions = positions.ToArray()
                }); ;
            }

            public Rotation3D GetRotation(Beacon beacon)
            {
                foreach(var baseRel in Relations)
                {
                    var potential = new List<BeaconRelation>();
                    foreach (var beaconRel in beacon.Relations)
                        //if hash match
                        if (baseRel.DistHash.Equals(beaconRel.DistHash))
                        {
                            //verify if can be orientated so that relayted matches
                            var orientationVector = beaconRel.Positions.FirstOrDefault(b => b.Equals(baseRel.MoveVect));
                            if (orientationVector != null)
                            {
                                return orientationVector.Rotation;
                            }
                        }

                }

                return null;
            }
        }

        private struct BeaconRelation
        {
            public Vector3D MoveVect;
            public string DistHash;
            public Beacon Target;
            public Vector3D[] Positions;
        }
    }
}
