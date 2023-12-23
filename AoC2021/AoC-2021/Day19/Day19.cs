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
        private IList<Scan> Input;
        public Day19(string filePath) : base(filePath)
        {
            var input = LineInput.GetEnumerator();
            Input = new List<Scan>();

            while (input.MoveNext())
            {
                var key = int.Parse(input.Current.Replace("scanner", "").Replace("---", "").Trim());
                string line;
                var beacons = new List<Beacon>();

                while (input.MoveNext() && (line = input.Current.Trim()) != string.Empty)
                {
                    var pos = line.Split(",").Select(l => long.Parse(l)).ToArray();
                    beacons.Add(new Beacon(new Vector3D(pos[0], pos[1], pos[2])));
                }

                Input.Add(new Scan(key, beacons));
            }
        }

        [ExpectedResult(TestName = "Example", Result = "79")]
        [ExpectedResult(TestName = "Input", Result = "467")]
        public override string Part1(string testName)
        {
            var fixedPos = new List<Scan>() { Input[0] };
            Input.Remove(Input[0]);



            return "";
        }

        [ExpectedResult(TestName = "Example", Result = "3621")]
        [ExpectedResult(TestName = "Input", Result = "12226")]
        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private class Scan
        {
            public readonly int Number;
            public IList<Beacon> Beacons { get; private set; }
            public bool Transformed = false;
            public Scan(int number, IList<Beacon> beacons)
            {
                Number = number;
                Beacons = beacons;
                foreach (var beacon in Beacons)
                    beacon.BuildRelated(Beacons);
            }

            public bool IsMatch(Scan other)
            {

                foreach(var tBeacon in Beacons)
                    foreach(var oBeacon in other.Beacons)
                    {
                        var matchRotation = tBeacon.IsMatch(oBeacon);
                        if (matchRotation)
                        {
                            return true;
                        }
                    }
                return false;
            }
        }
        private class Beacon
        {
            private Vector3D Pos;
            public IList<BeaconRelation> Relations;
            public long X => Pos.X;
            public long Y => Pos.Y;
            public long Z => Pos.Z;

            public Beacon(Vector3D vector)
            {
                Pos = vector;
            }

            private IEnumerable<long> dif(Beacon beacon)
            {
                yield return X - beacon.X;
                yield return Y - beacon.Y;
                yield return Z - beacon.Z;
            }

            public void BuildRelated(IList<Beacon> beacons)
            {
                Relations = new List<BeaconRelation>();
                foreach (var beacon in beacons.Where(b => b!= this))
                {
                    var diff = dif(beacon).ToArray();
                    var abs = diff.Select(d => Math.Abs(d)).ToArray();
                    var moveVect = new Vector3D(diff[0], diff[1], diff[2]);


                    Relations.Add(new BeaconRelation
                    {
                        MoveVect = moveVect,
                        DistHash = string.Join("|", abs.OrderBy(x => x)),
                        Target = beacon,
                        RotatedMoveVect = GetAllVectorRotations(moveVect).Distinct().ToList()
                    });
                    var a = 5;
                }
            }

            public bool IsMatch(Beacon other)
            {
                foreach(var tRelation in Relations)
                    foreach(var oRelation in other.Relations)
                        if (tRelation.DistHash.Equals(oRelation.DistHash))
                        {
                            if (tRelation.RotatedMoveVect.Any(r =>
                                r.X == oRelation.MoveVect.X &&
                                r.Y == oRelation.MoveVect.Y &&
                                r.Z == oRelation.MoveVect.Z))
                                return true;
                        }
                return false;
            }

            public static IEnumerable<Vector3D> GetAllVectorRotations(Vector3D start)
            {
                var vec = new Vector3D(start.X, start.Y, start.Z);
                for (var z = 0; z < 4; z++)
                {
                    vec.RotateCW(Axis.Z);
                    for (var y = 0; y < 4; y++)
                    {
                        vec = vec.RotateCW(Axis.Y);
                        for (var x = 0; x < 4; x++)
                        {
                            vec = vec.RotateCW(Axis.X);
                            yield return vec;
                        }
                    }
                }

            }

            /*public Vector3D GetTransformation(Beacon beacon)
            {
                foreach (var baseRel in Relations)
                {
                    foreach (var beaconRel in beacon.Relations)
                        //if hash match
                        if (baseRel.DistHash.Equals(beaconRel.DistHash))
                        {
                            //verify if can be orientated so that relayted matches
                            var orientationVector = beaconRel.RotatedMoveVect.FirstOrDefault(v => v.Equals(baseRel.MoveVect));
                            if (orientationVector != null)
                            {
                                var result = new Vector3D(beacon.X, beacon.Y, beacon.Z);

                                for (var x = 0; x * 90 < orientationVector.Rotation.X; x++)
                                    result = result.RotateCW(Axis.X);
                                for (var y = 0; y * 90 < orientationVector.Rotation.Y; y++)
                                    result = result.RotateCW(Axis.Y);
                                for (var z = 0; z * 90 < orientationVector.Rotation.Z; z++)
                                    result = result.RotateCW(Axis.Z);

                                return result;
                            }
                        }
                }
                return null;
            }*/
        }

        private struct BeaconRelation
        {
            public Vector3D MoveVect;
            public string DistHash;
            public Beacon Target;
            public IList<Vector3D> RotatedMoveVect;
        }
    }
}
