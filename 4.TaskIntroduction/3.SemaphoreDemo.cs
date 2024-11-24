namespace TaskIntroduction;

public class SemaphoreDemo
{
    // thread safe queue
    private Queue<string?> _requestQueue = new();
    private readonly Lock _padLock = new();

    // only 3 threads can access the resource at a time
    private SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);

    // SemaphoreSlim is used in process synchronization
    // Semaphore is used in inter process synchronization

    ~SemaphoreDemo()
    {
        _semaphore.Dispose();
    }

    public void Execute()
    {
        // main thread

        // 2. Start the requests queue monitoring thread
        Task monitoringTask = new(action: MonitorQueue);
        monitoringTask.Start();

        // 1. Enqueue the requests
        Console.WriteLine("Server is running. Type 'exit' to stop.");
        while (true)
        {
            string? input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
                break;
            }

            lock (_padLock)
            {
                _requestQueue.Enqueue(input);
            }
        }
    }

    // runs in a separate thread
    private void MonitorQueue()
    {
        while (true)
        {
            if (_requestQueue.Count > 0)
            {
                string? input;
                lock (_padLock)
                {
                    // processing done in a separate thread
                    var result = _requestQueue.TryDequeue(out input) ? input : null;
                }

                _semaphore.Wait();
                Task processingTask = new(() => ProcessInput(input));
                processingTask.Start();
            }
            Thread.Sleep(100);
        }
    }

    // 3. Processing the requests
    private void ProcessInput(string? input)
    {
        try
        {
            // Simulate processing time
            Thread.Sleep(2000);
            Console.WriteLine($"Processed input: {input}");
        }
        finally
        {
            var prevCount = _semaphore.Release();
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} released the semaphore. Previous count is: {prevCount}");
        }
    }
}