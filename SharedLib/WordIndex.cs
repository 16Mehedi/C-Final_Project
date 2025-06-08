using System;
using System.Collections.Generic;

namespace SharedLib
{
    [Serializable]
    public class WordIndex
    {
        public required string FileName { get; set; }
        public Dictionary<string, int> WordCounts { get; set; } = new();
    }
}
