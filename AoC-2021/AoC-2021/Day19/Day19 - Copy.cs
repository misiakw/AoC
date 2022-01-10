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
    public class Day19Copy : DayBase
    {
        private IList<Scan> Input;
        public Day19Copy(string filePath) : base(filePath)
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

                foreach(var beacon in beacons)
                    foreach (var other in beacons.Where(b => b != beacon))
                        beacon.AddRelated(other);

                Input.Add(new Scan(key, beacons));
            }
        }

        [ExpectedResult(TestName = "Example", Result = "79")]
        //[ExpectedResult(TestName = "Input", Result = 571032)]
        public override string Part1(string testName)
        {
            BuildMatchRelations();
            Input[0].Transformed = true;
            var beacons = TranslateAndMerge(Input[0]);
            //return beacons.Count().ToString();
            return "";
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }


        private IList<Scan> Orientate(Scan source, IList<Scan> remains)
        {
            var result = new List<Scan>() { source };
            if (!remains.Any()) return result;

            var newMatch = new List<Scan>();
            Vector3D vector = null;
            do
            {
                vector = null;
                foreach (var match in remains)
                {
                    vector = GetMatchingVector(source, match);
                    if (vector != null)
                    {
                        Console.WriteLine($"match {source.Number} with {match.Number}");
                        //translate match
                        newMatch.Add(match);
                        remains.Remove(match);
                        break;
                    }
                }
            } while (vector != null);

            foreach (var scan in newMatch)
                foreach (var orientated in Orientate(scan, remains))
                    if (orientated != null)
                        result.Add(orientated);

            return result;
        }


        private IList<Beacon> TranslateAndMerge(Scan scan)
        {
            var unprocessedScans = scan.Matching.Where(m => !m.Scan.Transformed).ToList();

            if(unprocessedScans.Count() == 0)
            {
                return scan.Beacons;
            }
            else
            {
                var result = scan.Beacons.ToList();
                foreach(var unprocessed in unprocessedScans)
                {
                    Console.WriteLine($"{scan.Number} => {unprocessed.Scan.Number}");
                    //transform
                    unprocessed.Scan.Transformed = true;

                    TranslateAndMerge(unprocessed.Scan);
                }
                return result;
            }
        }

        private void BuildMatchRelations()
        {
            for (var c = 0; c < Input.Count() - 1; c++)
            {
                for (var m = c + 1; m < Input.Count(); m++)
                {
                    var matching = GetMatchingVector(Input[c], Input[m]);
                    if (matching != null)
                    {
                        var oposite = new Vector3D(-1 * matching.X, -1 * matching.Y, -1 * matching.Z,
                            new Rotation3D(matching.Rotation.X+180, matching.Rotation.Y+180, matching.Rotation.Z+180));

                        Input[c].Matching.Add(new ScanRelation
                        {
                            Scan = Input[m],
                            Transform = oposite
                        });
                        Input[m].Matching.Add(new ScanRelation
                        {
                            Scan = Input[c],
                            Transform = matching
                        });
                        Console.WriteLine($"{c} matching {m}");
                    }
                }
            }
        }

        private Vector3D GetMatchingVector(Scan current, Scan match)
        {
            var unoriented = GetBeaconsVectorMatch(current.Beacons, match.Beacons).ToList();
            var topGroup = unoriented.GroupBy(u => u.Item3.Rotation.ToString())
                .Where(g => g.Count() >= 12).FirstOrDefault();
            return topGroup?.FirstOrDefault()?.Item3 ?? null;
        }

        private IEnumerable<Tuple<Beacon, Beacon, Vector3D>> GetBeaconsVectorMatch(IList<Beacon> current, IList<Beacon> match)
        {
            foreach(var beacon in current)
            {
                var potential = new List<Beacon>();
                foreach (var pretender in match)
                {
                    var ransformation = beacon.GetTransformation(pretender);
                    if (ransformation != null)
                        yield return Tuple.Create(beacon, pretender, ransformation);
                }
            }
        }

        private class Scan
        {
            public readonly int Number;
            public IList<Beacon> Beacons { get; private set; }
            public IList<ScanRelation> Matching = new List<ScanRelation>();
            public bool Transformed = false;
            public Scan(int number, IList<Beacon> beacons)
            {
                Number = number;
                Beacons = beacons;
            }
        }
        private class Beacon
        {
            private Vector3D Pos;
            private IList<BeaconRelation> Relations = new List<BeaconRelation>();
            public long X => Pos.X;
            public long Y => Pos.Y;
            public long Z => Pos.Z;
            
            public Beacon(Vector3D vector)
            {
                Pos = vector;
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


                Relations.Add(new BeaconRelation
                {
                    MoveVect = moveVect,
                    DistHash = string.Join("|", abs.OrderBy(x => x)),
                    Target = beacon
                }); ; ;
            }

            public Vector3D GetRotatedVector(Vector3D next, Vector3D match)
            {
                //get all rotation twists across Z rotation
                for (var zRot = 0; zRot < 4; zRot++)
                {
                    next = next.RotateCW(Axis.Z);
                    for (var rotX = 0; rotX < 4; rotX++)
                    {
                        next = next.RotateCW(Axis.X);
                        if (next.Equals(match)) return next;
                    }
                }
                //get twist across Y rotation 90 deg
                next = next.RotateCW(Axis.Y);
                for (var rotZ = 0; rotZ < 4; rotZ++)
                {
                    next = next.RotateCW(Axis.Z);
                    if (next.Equals(match)) return next;
                }
                //get twist across Y rotation 270 deg
                next = next.RotateCW(Axis.Y).RotateCW(Axis.Y);
                for (var rotZ = 0; rotZ < 4; rotZ++)
                {
                    next = next.RotateCW(Axis.Z);
                    if (next.Equals(match)) return next;
                }
                return null;
            }

            public Vector3D GetTransformation(Beacon beacon)
            {
                foreach(var baseRel in Relations)
                {
                    foreach (var beaconRel in beacon.Relations)
                        //if hash match
                        if (baseRel.DistHash.Equals(beaconRel.DistHash))
                        {
                            //verify if can be orientated so that relayted matches
                            var orientationVector = beacon.GetRotatedVector(beaconRel.MoveVect, baseRel.MoveVect);
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
            }

            public Beacon Transform(Vector3D vector)
            {
                var tmp = new Vector3D(X, Y, Z);

                for (var x = 0; x * 90 < vector.Rotation.X; x++)
                    tmp = tmp.RotateCW(Axis.X);
                for (var y = 0; y * 90 < vector.Rotation.Y; y++)
                    tmp = tmp.RotateCW(Axis.Y);
                for (var z = 0; z * 90 < vector.Rotation.Z; z++)
                    tmp = tmp.RotateCW(Axis.Z);

                var pos =  new Vector3D(X-tmp.X, Y-tmp.Y, Z-tmp.Z);
                return new Beacon(pos);
            }
        }

        private struct BeaconRelation
        {
            public Vector3D MoveVect;
            public string DistHash;
            public Beacon Target;
        }

        private struct ScanRelation
        {
            public Vector3D Transform;
            public Scan Scan;
        }
    }
}
