namespace ParallelLoops;

public class CancellingLoop
{
    private readonly CancellationTokenSource cts = new();

    ~CancellingLoop()
    {
        cts.Dispose();
    }

    public void Execute()
    {
        var token = cts.Token;

        var task = Task.Run(Work, token);

        Console.WriteLine("To cancel, press 'c'");
        var input = Console.ReadLine();
        if (input == "c")
        {
            cts.Cancel();
        }

        task.Wait();
        Console.WriteLine($"Task status is: {task.Status}");
        Console.ReadLine();
    }

    private void Work()
    {
        Console.WriteLine("Started doing the work.");

        var parallelOptions = new ParallelOptions { CancellationToken = cts.Token };

        try
        {
            Parallel.For(
                fromInclusive: 0,
                toExclusive: 100_000,
                parallelOptions: parallelOptions,
                body: i =>
            {
                Console.WriteLine($"{DateTime.Now}");
                Thread.SpinWait(30000000);
            });
        }
        catch (AggregateException ex)
        {
            Console.WriteLine(ex.ToString());
        }

        Console.WriteLine("Work is done.");
    }
}