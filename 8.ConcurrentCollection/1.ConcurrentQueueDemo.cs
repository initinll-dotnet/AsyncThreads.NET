using System.Collections.Concurrent;

namespace ConcurrentCollection;

public class ConcurrentQueueDemo
{
    ConcurrentQueue<string?> requestQueue = new();

    public void Exceute()
    {
        // 2. Start the requests queue monitoring thread
        Thread monitoringThread = new(MonitorQueue);

        monitoringThread.Start();

        // 1. Enqueue the requests
        Console.WriteLine("Server is running. Type 'exit' to stop.");
        while (true)
        {
            string? input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
                break;
            }

            requestQueue.Enqueue(input);
        }
    }

    void MonitorQueue()
    {
        while (true)
        {
            // no need to use lock as ConcurrentQueue is thread-safe
            if (requestQueue.Count > 0)
            {
                if (requestQueue.TryDequeue(out var input))
                {
                    Thread processingThread = new Thread(() => ProcessInput(input));
                    processingThread.Start();
                }
            }
            Thread.Sleep(100);
        }
    }

    // 3. Processing the requests
    void ProcessInput(string? input)
    {
        // Simulate processing time
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}");
    }
}