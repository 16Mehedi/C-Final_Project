using SharedLib;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        SetProcessorAffinity(2); // Core 2
        string dirPath = args.Length > 0 ? args[0] : "./textsB";
        string pipeName = args.Length > 1 ? args[1] : "agent2";




        var thread = new Thread(() => ScanAndSend(dirPath, pipeName));
        thread.Start();
        thread.Join();
    }

    static void ScanAndSend(string dir, string pipe)
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

        using var client = new NamedPipeClientStream(".", pipe, PipeDirection.Out);
        client.Connect();

        var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
        formatter.Serialize(client, indexes);
#pragma warning restore SYSLIB0011
    }

    static void SetProcessorAffinity(int core)
    {
        Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(1 << core);
    }
}
