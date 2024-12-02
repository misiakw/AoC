using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoCBase2
{
    internal class PrintTable
    {
        private IList<CellClass[]> _rows = new List<CellClass[]>();
        private CellClass[] _selectedRow = null;
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
                    Console.ForegroundColor = row[coll].color;
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

        public PrintTable Row()
        {
            _selectedRow = new CellClass[_size];
            _rows.Add(_selectedRow);
            _selectedCell = 0;
            return this;
        }
        public PrintTable Cell(string text, ConsoleColor color = ConsoleColor.White)
        {
            _selectedRow[_selectedCell] = new CellClass()
            {
                value = text,
                color = color
            };
            if (_colls[_selectedCell].Length < text.Length)
                _colls[_selectedCell].Length = text.Length;
            _selectedCell++;
            return this;
        }

        internal class CellClass
        {
            public string value;
            public ConsoleColor color = ConsoleColor.White;
        }
        private class CollumnMeta
        {
            public string Header;
            public int Length;
        }
    }
}
