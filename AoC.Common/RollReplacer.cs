using System.Collections.Generic;

namespace AoC.Common
{
    public class RollReplacer
    {
        private RepNode _root = new RepNode();
        public RollReplacer(IList<(string, string)> replacements)
        {
            foreach (var item in replacements)
                this._root.Add(item.Item1, item.Item2);
        }

        public string Replace(string input)
        {
            var result = string.Empty;
            var buff = new Queue<char>();

            foreach (var ch in input)
            {
                buff.Enqueue(ch);
                var currKey = new string(buff.ToArray());
                if (this._root.CanReachValue(currKey))
                {
                    var val = this._root.GetValue(currKey);
                    if (val != null)
                    {
                        result = result + val;
                        buff.Clear();
                    }
                }
                else
                {
                    while (buff.Count > 0 && !this._root.CanReachValue(currKey))
                    {
                        result += buff.Dequeue();
                        currKey = new string(buff.ToArray());
                        if (this._root.CanReachValue(currKey))
                        {
                            var val = this._root.GetValue(currKey);
                            if (val != null)
                            {
                                result = result + val;
                                buff.Clear();
                            }
                        }
                    }
                }
            }
            while (buff.Count > 0)
                result += buff.Dequeue();

            return result;
        }

        private class RepNode
        {
            public IDictionary<char, RepNode> Nodes = new Dictionary<char, RepNode>();
            private string? _value = null;

            public void Add(string key, string value)
            {
                if (string.IsNullOrEmpty(key))
                    this._value = value;
                else
                {
                    var newK = key[0];
                    if (!Nodes.ContainsKey(newK))
                        Nodes.Add(newK, new RepNode());
                    Nodes[newK].Add(key.Substring(1), value);
                }
            }

            public bool CanReachValue(string key)
            {
                if (string.IsNullOrEmpty(key))
                    return true;
                if (!this.Nodes.ContainsKey(key[0]))
                    return false;
                return Nodes[key[0]].CanReachValue(key.Substring(1));
            }
            public string? GetValue(string key)
                => string.IsNullOrEmpty(key)
                ? this._value
                : this.Nodes[key[0]].GetValue(key.Substring(1));
        }
    }
}
