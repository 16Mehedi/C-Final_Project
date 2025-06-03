using System;
using System.Collections.Generic;

namespace SharedLib
{
    [Serializable]
    public class WordIndex
    {
        public string FileName { get; set; }
        public Dictionary<string, int> WordCounts { get; set; } = new();
    }
}
