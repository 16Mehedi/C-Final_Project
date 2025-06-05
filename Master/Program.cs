using SharedLib;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;

class Program
{
    static void Main()
    {
        SetProcessorAffinity(3); // Core 3

        Console.WriteLine("Waiting for AgentA...");
        var threadA = new Thread(() => ReceiveFromPipe("agent1"));
        threadA.Start();

        Console.WriteLine("Waiting for AgentB...");
        var threadB = new Thread(() => ReceiveFromPipe("agent2"));
        threadB.Start();

        threadA.Join();
        threadB.Join();

        PrintResults();
        Console.WriteLine("\n? Done. Press any key to exit.");
        Console.ReadKey();
    }

    static readonly List<WordIndex> AllResults = new();
    static readonly object LockObj = new();

    static void ReceiveFromPipe(string pipeName)
    {
        using var server = new NamedPipeServerStream(pipeName, PipeDirection.In);
        server.WaitForConnection();

        var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
        var data = (List<WordIndex>)formatter.Deserialize(server);
#pragma warning restore SYSLIB0011

        lock (LockObj)
        {
            AllResults.AddRange(data);
        }
    }

    static void PrintResults()
    {
        Console.WriteLine("\nThe master process prints a consolidated word index:");
        foreach (var index in AllResults)
        {
            foreach (var kv in index.WordCounts)
                Console.WriteLine($"{index.FileName}: {kv.Key}: {kv.Value}");
        }
    }

    static void SetProcessorAffinity(int core)
    {
        Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(1 << core);
    }
}
