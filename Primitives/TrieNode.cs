namespace Internals.Primitives
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;


    class TrieNode<T> :
        IEnumerable<T>
    {
        public readonly char Key;
        public readonly TrieNode<T> Parent;


        readonly Dictionary<char, TrieNode<T>> _nodes;
        bool _hasValue;
        T _value;

        public TrieNode(TrieNode<T> parent, char key)
        {
            Parent = parent;
            Key = key;

            _hasValue = false;
            _nodes = new Dictionary<char, TrieNode<T>>();
        }

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _hasValue = true;
            }
        }

        public int Count
        {
            get { return _nodes.Count; }
        }

        public bool IsEmpty
        {
            get { return _nodes.Count == 0; }
        }

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public TrieNode<T> this[char key]
        {
            get
            {
                TrieNode<T> node;
                if (_nodes.TryGetValue(key, out node))
                    return node;

                node = new TrieNode<T>(this, key);
                _nodes.Add(key, node);
                return node;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty)
                return Enumerable.Repeat(Value, HasValue ? 1 : 0).GetEnumerator();

            return _nodes.Values.SelectMany(node => node)
                         .Concat(Enumerable.Repeat(Value, HasValue ? 1 : 0)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void RemoveValue()
        {
            _hasValue = false;
            _value = default(T);
        }

        public bool TryGetNode(char key, out TrieNode<T> child)
        {
            return _nodes.TryGetValue(key, out child);
        }

        public void Remove(char key)
        {
            _nodes.Remove(key);
        }

        public IList<T> All()
        {
            var values = new List<T>();

            All(values);

            return values;
        }

        void All(List<T> values)
        {
            if (IsEmpty)
            {
                if (HasValue)
                {
                    values.Add(Value);
                    return;
                }

                return;
            }

            foreach (var node in _nodes.Values)
                node.All(values);

            if (HasValue)
                values.Add(Value);
        }
    }
}