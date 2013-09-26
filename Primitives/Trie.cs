namespace Internals.Primitives
{
    /// <summary>
    /// A Trie data structure for 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Trie<T>
    {
        readonly TrieNode<T> _root;

        public Trie()
        {
            _root = new TrieNode<T>(null, '\0');
        }

        /// <summary>
        /// Add a key to the Trie if it does not exist and set the value for the key
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="value">The value to store at the key position</param>
        public void Add(string key, T value)
        {
            TrieNode<T> node = _root;
            foreach (char c in key)
                node = node[c];

            node.Value = value;
        }

        /// <summary>
        /// Remove a key from the Trie, as well as the value at the key position. Any empty nodes along the key path 
        /// are removed as well.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            TrieNode<T> node = _root;
            foreach (char c in key)
                node = node[c];

            node.RemoveValue();

            while (node != _root && !node.HasValue && node.Count == 0)
            {
                char previousKey = node.Key;
                node = node.Parent;
                node.Remove(previousKey);
            }
        }

        /// <summary>
        /// Returns a walker positioned at the root of the Trie
        /// </summary>
        /// <returns></returns>
        public TrieWalker<T> Root()
        {
            return new TrieWalker<T>(_root);
        }

        /// <summary>
        /// Searches for the specified key. If the key is found, walker points to the key within the Trie. If
        /// the key is not found, the walker points to the portion of the key that was found (if any).
        /// </summary>
        /// <param name="key">The key to find</param>
        /// <param name="walker">The walker used to locate the key (even if the key was not found)</param>
        /// <returns>True if the key was found, otherwise false.</returns>
        public bool TryFind(string key, out TrieWalker<T> walker)
        {
            walker = new TrieWalker<T>(_root);
            foreach (char c in key)
            {
                if (!walker.Next(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}