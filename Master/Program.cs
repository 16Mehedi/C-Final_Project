using SharedLib;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;

class Program
{
    static List<WordIndex> allIndexes = new();

    static void Main()
    {
        SetProcessorAffinity(2);
        Console.WriteLine("Waiting for AgentA...");
        var t1 = new Thread(() => ReceiveFromPipe("agent1"));
        t1.Start();

        Console.WriteLine("Waiting for AgentB...");
        var t2 = new Thread(() => ReceiveFromPipe("agent2"));
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine("\nThe master process prints a consolidated word index:");
        PrintResults(allIndexes);
        Console.WriteLine("\n✅ Done. Press any key to exit.");
        Console.ReadKey();
    }

    static void ReceiveFromPipe(string pipeName)
    {
        try
        {
            using var server = new NamedPipeServerStream(pipeName, PipeDirection.In);
            server.WaitForConnection();

            var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            var indexes = (List<WordIndex>)formatter.Deserialize(server);
#pragma warning restore SYSLIB0011

            lock (allIndexes)
            {
                allIndexes.AddRange(indexes);
            }

            Console.WriteLine($"✅ Received data from {pipeName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error receiving from {pipeName}: {ex.Message}");
        }
    }

    static void PrintResults(List<WordIndex> indexes)
    {
        foreach (var index in indexes)
        {
            foreach (var kv in index.WordCounts)
            {
                Console.WriteLine($"{index.FileName}: {kv.Key}: {kv.Value}");
            }
        }
    }

    static void SetProcessorAffinity(int core)
    {
        Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(1 << core);
    }
}
