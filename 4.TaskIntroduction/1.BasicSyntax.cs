namespace TaskIntroduction;

public class BasicSyntax
{
    // threads are non blocking in nature
    // main thread is blocking in nature
    public void Execute()
    {
        Console.WriteLine("Main thread before starting task: " + Environment.CurrentManagedThreadId);

        var task = Task.Run(WriteThreadId);

        task.GetAwaiter().OnCompleted(() =>
        {
            Console.WriteLine($"Task completed on thread: {Environment.CurrentManagedThreadId}");
        });

        Console.WriteLine("Main thread after starting task: " + Environment.CurrentManagedThreadId);

        Task.Delay(5000).Wait();
    }

    private void WriteThreadId()
    {
        var sum = 0;
        for (int i = 0; i < 100; i++)
        {
            sum += i;
            Thread.Sleep(5);
        }
        Console.WriteLine($"Sum {sum} calculated on Thread ID: {Environment.CurrentManagedThreadId}");
    }
}
