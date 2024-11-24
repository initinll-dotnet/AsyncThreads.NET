using System.Collections.Concurrent;

namespace ThreadIntroduction;

public class WebServer
{
    // thread safe queue
    private ConcurrentQueue<string?> requestQueue;

    public WebServer()
    {
        requestQueue = new ConcurrentQueue<string?>();
    }

    public void Execute()
    {
        // main thread

        // 2. Start the requests queue monitoring thread
        Thread monitoringThread = new Thread(MonitorQueue);
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

    // runs in a separate thread
    private void MonitorQueue()
    {
        while (true)
        {
            if (requestQueue.Count > 0)
            {
                // processing done in a separate thread
                var result = requestQueue.TryDequeue(out string? input) ? input : null;
                Thread processingThread = new Thread(() => ProcessInput(input));
                processingThread.Start();
            }
            Thread.Sleep(100);
        }
    }

    // 3. Processing the requests
    private void ProcessInput(string? input)
    {
        // Simulate processing time
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}");
    }
}
