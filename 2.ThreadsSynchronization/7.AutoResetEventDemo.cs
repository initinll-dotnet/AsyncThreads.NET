namespace ThreadsSynchronization;

public class AutoResetEventDemo
{
    private AutoResetEvent autoResetEvent = new(initialState: false);

    string? userInput = null;

    ~AutoResetEventDemo()
    {
        autoResetEvent.Dispose();
    }

    public void Execute()
    {
        // Start the worker thread
        StartWorkerThreads();
        // Main thread receives user input
        GetUserInput();
        // Exit the application
        Environment.Exit(0);
    }

    private void StartWorkerThreads()
    {
        // Start the worker thread
        for (int i = 0; i < 3; i++)
        {
            Thread workerThread = new Thread(Worker);
            workerThread.Name = $"Worker {i + 1}";
            workerThread.Start();
        }
    }

    private void GetUserInput()
    {
        Console.WriteLine("Server is running. Type 'go' to proceed and  'exit' to stop.");

        while (true)
        {
            userInput = Console.ReadLine();

            // Signal the worker thread if input is "go"
            if (userInput?.ToLower() == "go")
            {
                autoResetEvent.Set();
            }

            if (userInput?.ToLower() == "exit")
            {
                Console.WriteLine("Exited ..");
                break;
            }
        }
    }

    private void Worker()
    {
        while (true)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for signal.");
            // Wait for the signal from the main thread
            autoResetEvent.WaitOne();

            Console.WriteLine($"{Thread.CurrentThread.Name} proceeds.");
            // Simulate processing time
            Thread.Sleep(2000);
        }
    }
}
