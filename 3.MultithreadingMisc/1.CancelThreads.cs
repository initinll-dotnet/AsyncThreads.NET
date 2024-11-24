namespace MultithreadingMisc;

public class CancelThreads
{
    bool cancelThread = false;

    public void Execute()
    {
        Thread thread = new(start: Work);
        thread.Start();

        Console.WriteLine("To cancel, press 'c'");
        var input = Console.ReadLine();
        if (input == "c")
        {
            cancelThread = true;
        }

        thread.Join();
        Console.ReadLine();
    }

    private void Work()
    {
        Console.WriteLine("Started doing the work.");

        for (int i = 0; i < 100_000; i++)
        {
            if (cancelThread)
            {
                Console.WriteLine($"User requested cancellation at iteration: {i}");
                break;
            }

            Thread.SpinWait(300_000);
        }

        Console.WriteLine("Work is done.");
    }
}
