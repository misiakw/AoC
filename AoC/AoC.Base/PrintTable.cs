using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Base
{
    internal class PrintTable
    {
        private IList<RowMeta> _rows = new List<RowMeta>();
        private RowMeta _selectedRow = null;
        private int _selectedCell = 0;
        private CollumnMeta[] _colls;
        private int _size;

        public PrintTable(IList<string> headers)
        {
            _colls = headers.Select(h => new CollumnMeta() { Header = h, Length = h.Length}).ToArray();
            _size = headers.Count;
        }

        public void PrintConsole()
        {
            Console.WriteLine(string.Join(" | ", _colls.Select(c => c.Header.PadRight(c.Length))));
            var dash = new List<string>() { "".PadRight(_colls[0].Length + 1, '-') };
            dash.AddRange(_colls.Skip(1).Select(c => "".PadRight(c.Length + 2, '-')));
            Console.WriteLine(string.Join("+", dash));
            foreach (var row in _rows)
            {
                for (var coll = 0; coll < _size; coll++)
                {
                    Console.ForegroundColor = row[coll].color ?? row.Color;
                    Console.Write(row[coll].value.PadRight(_colls[coll].Length));
                    Console.ForegroundColor = ConsoleColor.White;
                    if (coll < _size - 1)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
            return;
        }

        public PrintTable Row(ConsoleColor color = ConsoleColor.White)
        {
            _selectedRow = new RowMeta(_size, color);
            _rows.Add(_selectedRow);
            _selectedCell = 0;
            return this;
        }
        public PrintTable Cell(string text, ConsoleColor? color = null) 
            => Cell(text, _selectedCell++, color);
        public PrintTable Cell(string text, int collumn, ConsoleColor? color = null)
        {
            _selectedRow[collumn] = new CellClass()
            {
                value = text ?? string.Empty,
                color = color
            };
            if (_colls[collumn].Length < (text?.Length ?? 0))
                _colls[collumn].Length = text.Length;
            return this;
        }


        internal class CellClass
        {
            public string value;
            public ConsoleColor? color;
        }
        private class CollumnMeta
        {
            public string Header;
            public int Length;
        }
        private class RowMeta
        {
            public RowMeta(int size, ConsoleColor color)
            {
                _cellls = new CellClass[size];
                Color = color;
            }
            public readonly ConsoleColor Color;
            private CellClass[] _cellls;
            public CellClass this[int index] {
                get { return _cellls[index]; }
                set { _cellls[index] = value; } 
             }   
        }
    }
}
