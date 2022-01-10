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
        private IReadOnlyList<Scan> Input;
        public Day19(string filePath) : base(filePath)
        {
            var input = LineInput.GetEnumerator();
            var tmp = new List<Scan>();

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

                tmp.Add(new Scan(key, beacons));
            }
            Input = tmp;
        }

        [ExpectedResult(TestName = "Example", Result = "79")]
        [ExpectedResult(TestName = "Input", TooHigh = 500)]
        public override string Part1(string testName)
        {
            var scans = Input.ToList();

            while (SearchForOverlap(scans)) ;

            var beacons = new List<Beacon>();

            foreach (var scan in scans)
                foreach (var beacon in scan.Beacons)
                    beacons.Add(beacon);

            return (beacons.Select(b => b.ToString()).Distinct().Count() - (scans.Count() - 1) * 12).ToString();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private bool SearchForOverlap(IList<Scan> scans)
        {
            if (scans.Count() == 1) return false;

            for (var c = 0; c < scans.Count() - 1; c++) {
                var root = scans[c];
                for (var m = c + 1; m < scans.Count(); m++)
                {
                    var match = scans[m];
                    var overlapped = root.Overlap(match);
                    if (overlapped != null)
                    {
                        scans.Remove(root);
                        scans.Remove(match);
                        scans.Add(overlapped);
                        Console.WriteLine($"{scans.Count()}");
                        return true;
                    }
                }
            }
            return false;
        }

        private class Scan
        {
            public readonly int Number;
            public IList<Beacon> Beacons { get; private set; }
            public Scan(int number, IList<Beacon> beacons)
            {
                Number = number;
                Beacons = beacons;
                foreach (var beacon in beacons)
                    foreach (var other in beacons.Where(b => b != beacon))
                        beacon.AddRelated(other);
            }

            public Scan Overlap(Scan other)
            {
                var rotations = new List<Rotation>();
                foreach(var beacon in Beacons)
                    foreach( var otherBeacon in other.Beacons)
                    {
                        var oRotation = beacon.GetOrientationRotation(otherBeacon);
                        if(oRotation != null)
                        {
                            rotations.Add(oRotation.Value);
                        }
                    }

                var rotation = rotations.GroupBy(r => r.rotation.ToString()).Where(g => g.Count() >= 12).FirstOrDefault();

                if (rotation == null) return null;

                var matched = rotation.Select(r => r.source).ToList();
                var unmatched = other.Beacons.Where(b => !matched.Contains(b)).ToList();

                var source = new Beacon(Rotate(rotation.First().source.Pos, rotation.First().rotation));
                var target = rotation.First().target;
                //get position of other scanner in relate to this scanner
                var transform = new Vector3D(target.Pos.X - source.Pos.X, target.Pos.Y - source.Pos.Y, target.Pos.Z - source.Pos.Z);

                var vectors = Beacons.Select(b => b.Pos).ToList();

                foreach(var uPos in unmatched.Select(p => p.Pos))
                {
                    var tmp = Rotate(uPos, rotation.First().rotation);
                    vectors.Add(new Vector3D(tmp.X + transform.X, tmp.Y + transform.Y, tmp.Z + transform.Z));
                }

                return new Scan(Number, vectors.Select(v => new Beacon(v)).ToList());
            }

            public static Vector3D Rotate(Vector3D pos, Rotation3D rotation)
            {
                var tmp = pos;
                for (var i = 0; i * 90 < rotation.X; i++)
                    tmp = tmp.RotateCW(Axis.X);
                for (var i = 0; i * 90 < rotation.Y; i++)
                    tmp = tmp.RotateCW(Axis.Y);
                for (var i = 0; i * 90 < rotation.Z; i++)
                    tmp = tmp.RotateCW(Axis.Z);
                return tmp;
            }

            public IEnumerable<Vector3D> OrientateVector(Rotation3D rotation) 
            {
                foreach (var pos in Beacons.Select(b => b.Pos)) 
                    yield return Rotate(pos, rotation);
            }
        }
        private class Beacon
        {
            public readonly Vector3D Pos;
            public IList<BeaconRel> Related = new List<BeaconRel>();

            public Beacon(Vector3D vector)
            {
                Pos = vector;
            }
            public void AddRelated(Beacon other)
            {
                var pos = new long[3];
                pos[0] = other.Pos.X - Pos.X;
                pos[1] = other.Pos.Y - Pos.Y;
                pos[2] = other.Pos.Z - Pos.Z;

                Related.Add(new BeaconRel
                {
                    Transform = new Vector3D(pos[0], pos[1], pos[2]),
                    Target = other,
                    Hash = string.Join("|", pos.Select(p => Math.Abs(p)).OrderBy(p => p))
                }); 
            }
            public Rotation? GetOrientationRotation(Beacon other)
            {
                foreach(var rel in Related)
                    foreach(var otherRel in other.Related)
                        if (rel.Hash.Equals(otherRel.Hash))
                        {
                            var rotated = GetRotatedVector(otherRel.Transform, rel.Transform);
                            if (rotated != null)
                            {
                                return new Rotation
                                {
                                    source = other,
                                    target = this,
                                    rotation = rotated.Rotation
                                };
                            }
                        }
                return null;
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
            public override string ToString() => $"[{Pos.X},{Pos.Y},{Pos.Z}]";
        }

        private struct BeaconRel
        {
            public Vector3D Transform;
            public Beacon Target;
            public string Hash;
        }
        private struct Rotation
        {
            public Beacon source, target;
            public Rotation3D rotation;
        }
    }
}
