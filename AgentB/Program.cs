using SharedLib;
using System.Diagnostics;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        SetProcessorAffinity(2); // Core 2
        string dirPath = args.Length > 0 ? args[0] : "./textsB";

        var thread = new Thread(() => ScanAndDisplay(dirPath));
        thread.Start();
        thread.Join();
    }

    static void ScanAndDisplay(string dir)
    {
        var indexes = new List<WordIndex>();

        foreach (var file in Directory.GetFiles(dir, "*.txt"))
        {
            var content = File.ReadAllText(file);
            var words = Regex.Matches(content.ToLower(), @"\b\w+\b");
            var count = new Dictionary<string, int>();

            foreach (Match word in words)
            {
                count[word.Value] = count.TryGetValue(word.Value, out int c) ? c + 1 : 1;
            }

            indexes.Add(new WordIndex
            {
                FileName = Path.GetFileName(file),
                WordCounts = count
            });
        }

        foreach (var index in indexes)
        {
            Console.WriteLine($"AgentB - File: {index.FileName}");
            foreach (var kv in index.WordCounts)
                Console.WriteLine($"  {kv.Key}: {kv.Value}");
        }
    }

    static void SetProcessorAffinity(int core)
    {
        Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(1 << core);
    }
}
