namespace Internals.Primitives
{
    using System.Collections;
    using System.Collections.Generic;


    class TrieWalker<T> :
        IEnumerable<T>
    {
        readonly TrieNode<T> _root;
        TrieNode<T> _current;
        string _prefix;

        public TrieWalker(TrieNode<T> root)
        {
            _root = root;
            _current = root;
        }

        void Reset()
        {
            _current = _root;
            _prefix = null;
        }


        public char Key
        {
            get { return _current.Key; }
        }

        public bool Next(char key)
        {
            TrieNode<T> child;
            if (_current.TryGetNode(key, out child))
            {
                _current = child;
                _prefix += key;
                return true;
            }

            return false;
        }

        public void Previous()
        {
            if (_current != _root)
            {
                _current = _current.Parent;
                _prefix = _prefix.Substring(0, _prefix.Length - 1);
            }
        }

        public IList<T> GetMatches()
        {
            return _current.All();
        }

        public bool HasValue
        {
            get { return _current.HasValue; }
        }

        public T Value
        {
            get { return _current.HasValue ? _current.Value : default(T); }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _current.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}