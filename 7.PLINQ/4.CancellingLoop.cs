namespace PLINQ;

public class CancellingLoop
{
    public void Execute()
    {
        using var cts = new CancellationTokenSource();

        var items = Enumerable.Range(1, 20);

        ParallelQuery<int> evenNumbers = null;

        evenNumbers = items
        .AsParallel()
        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
        .WithCancellation(cts.Token)
        .Where(x =>
        {
            Console.WriteLine($"Processing number {x}; Thread Id: {Thread.CurrentThread.ManagedThreadId}");

            // if (x == 5) throw new InvalidOperationException("This is intentional 5");

            // if (x == 20) throw new ArgumentNullException("This is intentional 20");

            return (x % 2 == 0);
        });


        Console.WriteLine();

        try
        {
            evenNumbers.ForAll(item =>
            {
                if (item > 8)
                {
                    cts.Cancel();
                }
                Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            });
        }
        catch (OperationCanceledException ex)
        {
            // catches the exception thrown by the cancellation token
            Console.WriteLine(ex.Message);
        }
        catch (AggregateException ex)
        {
            // catches exception thrown by threads
            ex.Handle(x =>
            {
                Console.WriteLine(x.Message);
                return true;
            });
        }
        catch (Exception ex)
        {
            // catches exception thrown by the query
            Console.WriteLine(ex.Message);
        }
    }
}