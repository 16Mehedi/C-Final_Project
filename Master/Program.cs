using SharedLib;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text.Json;

class Program
{
    static void Main()
    {
        SetProcessorAffinity(0); // Use core 0 for master

        var allIndexes = new List<WordIndex>();

        Console.WriteLine("Waiting for AgentA...");
        allIndexes.AddRange(ReceiveFromPipe("agent1"));

        Console.WriteLine("Waiting for AgentB...");
        allIndexes.AddRange(ReceiveFromPipe("agent2"));

        Console.WriteLine("\nThe master process prints a consolidated word index:");
        foreach (var index in allIndexes)
        {
            foreach (var kv in index.WordCounts)
            {
                Console.WriteLine($"{index.FileName}: {kv.Key}: {kv.Value}");
            }
        }

        Console.WriteLine("\n✅ Done. Press any key to exit.");
        Console.ReadKey();
    }

    static List<WordIndex> ReceiveFromPipe(string pipeName)
    {
        // Accept 1 instance (which is correct for our simple setup)
        using var server = new NamedPipeServerStream(
            pipeName,
            PipeDirection.In,
            1, // max server instances
            PipeTransmissionMode.Byte,
            PipeOptions.None // synchronous and safe
        );

        server.WaitForConnection();

        return JsonSerializer.Deserialize<List<WordIndex>>(server) ?? new();
    }

    static void SetProcessorAffinity(int core)
    {
        if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(1 << core);
        }
    }
}
