namespace MultithreadingMisc;

public class ThreadPoolDemo
{
    // thread safe queue
    Queue<string?> requestQueue = new();

    public void Execute()
    {
        // main thread

        ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
        ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
        ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableCompletionPortThreads);

        Console.WriteLine($"Max Worker Threads: {maxWorkerThreads}, Max Completion Port Threads: {maxCompletionPortThreads}");
        Console.WriteLine($"Min Worker Threads: {minWorkerThreads}, Min Completion Port Threads: {minCompletionPortThreads}");

        Console.WriteLine($"Available Worker Threads: {maxWorkerThreads - availableWorkerThreads}, Available Completion Port Threads: {maxCompletionPortThreads - availableCompletionPortThreads}");

        // 2. Start the requests queue monitoring thread
        Thread monitoringThread = new Thread(start: MonitorQueue);
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
                // requesting thread pool to process the input
                ThreadPool.QueueUserWorkItem(ProcessInput, input);
            }
            Thread.Sleep(100);
        }
    }

    // 3. Processing the requests
    private void ProcessInput(object? input)
    {
        // Simulate processing time
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}. Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
    }
}
