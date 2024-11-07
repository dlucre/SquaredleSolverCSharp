using System;
using System.Collections.Generic;
using System.IO;

namespace SquaredleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load dictionary
            Console.WriteLine("Loading dictionary...");
            var dictionaryPath = "words.txt"; // Update the path as necessary
            var wordsList = File.ReadAllLines(dictionaryPath);
            var trie = new Trie();
            foreach (var word in wordsList)
            {
                trie.Insert(word.ToLower());
            }
            Console.WriteLine("Dictionary loaded.");

            // Create grid
            var grid = CreateGrid();

            // Find words
            Console.WriteLine("Finding words...");
            var foundWords = new HashSet<string>();
            FindWords(grid, trie, foundWords);

            // Display results
            Console.WriteLine("Found words:");
            foreach (var word in foundWords.OrderBy(a => a.Length))
            {
                Console.WriteLine(word);
            }
        }

        static char[,] CreateGrid()
        {
            // Example 4x4 grid
            return new char[,]
            {
                { 'f', 'c', 't', 'u' },
                { 'l', 'a', 'r', 'a' },
                { 'y', 'e', 'r', 'c' },
                { 'n', 'l', 'i', 'm' }
            };
        }

        static void FindWords(char[,] grid, Trie trie, HashSet<string> foundWords)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            bool[,] visited = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    DFS(grid, visited, "", i, j, trie, foundWords);
                }
            }
        }

        static void DFS(char[,] grid, bool[,] visited, string currentWord, int row, int col, Trie trie, HashSet<string> foundWords)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            if (row < 0 || col < 0 || row >= rows || col >= cols || visited[row, col])
                return;

            currentWord += grid[row, col];
            currentWord = currentWord.ToLower();

            if (!trie.StartsWith(currentWord))
                return;

            if (currentWord.Length >= 4 && trie.Search(currentWord))
                foundWords.Add(currentWord);

            visited[row, col] = true;

            // Explore all adjacent cells including diagonals
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                        DFS(grid, visited, currentWord, row + i, col + j, trie, foundWords);
                }
            }

            visited[row, col] = false;
        }
    }
    class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; set; } = new Dictionary<char, TrieNode>();
        public bool IsWord { get; set; } = false;
    }

    class Trie
    {
        private TrieNode root = new TrieNode();

        public void Insert(string word)
        {
            var node = root;
            foreach (var ch in word)
            {
                if (!node.Children.ContainsKey(ch))
                    node.Children[ch] = new TrieNode();
                node = node.Children[ch];
            }
            node.IsWord = true;
        }

        public bool StartsWith(string prefix)
        {
            var node = root;
            foreach (var ch in prefix)
            {
                if (!node.Children.ContainsKey(ch))
                    return false;
                node = node.Children[ch];
            }
            return true;
        }

        public bool Search(string word)
        {
            var node = root;
            foreach (var ch in word)
            {
                if (!node.Children.ContainsKey(ch))
                    return false;
                node = node.Children[ch];
            }
            return node.IsWord;
        }
    }


}
