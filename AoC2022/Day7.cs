using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day7 : DayBase
    {
        public Day7() : base(7)
        {
            Input("example1")
                .RunPart(1, 95437L)
                .RunPart(2, 24933642L)
            .Input("output")
                .RunPart(1, 1118405L)
                .RunPart(2, 12545514L);
        }

        public override object Part1(Input input)
        {
            var root = new DiskDir("/", null);
            var pwd = root;

            foreach(var line in input.Lines.Select(l => l.Split(" "))){
                if (line[0] == "$"){ //process CMD
                    if(line[1].ToLower() == "cd"){
                        if(line[2] == "/")
                            pwd = root;
                        else if (line[2] == "..")
                            pwd = pwd?.Root;
                        else
                            pwd = pwd?.Directories.First(d => d.Name == line[2]);
                    }else{
                        //ls, do nothing
                    }
                }else{
                    if(line[0] == "dir")
                        pwd?.Directories.Add(new DiskDir(line[1], pwd));
                    else
                        pwd?.Files.Add(new DiskFile(line[1], long.Parse(line[0])));
                }
            }

            input.Cache = root;

            var dirs = root.ListDirs().Where(d => d.Size <= 100000).ToList();
            return dirs.Sum(d => d.Size);
        }

        public override object Part2(Input input)
        {
             var root = (DiskDir?) (input.Cache ?? new DiskDir("/", null));
             var needed = 30000000L-(70000000L-root?.Size);

             var dirs = root.ListDirs().Where(d => d.Size >= needed).OrderBy(d => d.Size);
             return dirs.First().Size;
        }

        private abstract class DiskStruct{
            public string Name { get; }
            public abstract long Size { get; }
            public DiskStruct(string name){
                this.Name = name;
            }
        }

        private class DiskFile : DiskStruct
        {
            private long _size;
            public override long Size => _size;

            public DiskFile(string name, long size): base(name){
                this._size = size;
            }
        }

        private class DiskDir : DiskStruct
        {
            public override long Size => Files.Sum(f => f.Size) + Directories.Sum(d => d.Size);
            public IList<DiskDir> Directories = new List<DiskDir>();
            public IList<DiskFile> Files = new List<DiskFile>();
            public readonly DiskDir? Root = null;

            public DiskDir(string name, DiskDir? root): base(name){
                this.Root = root;
            }

            public IList<DiskDir> ListDirs(){
                var result = new List<DiskDir>();
                result.AddRange(Directories);
                foreach(var dir in Directories){
                    result.AddRange(dir.ListDirs());
                }
                return result;
            }
        }
    }
}
