namespace ParallelLoops;

public class ExceptionHandling
{
    private readonly Lock _lock = new();

    public void Execute()
    {
        // The loop will stop executing as soon as the exception is encountered.
        // The exceptions are aggregated into an AggregateException that will be thrown after the loop ends.
        // all three methods Parallel.For, Parallel.ForEach & Parallel.Invoke are blocking and synchronous
        try
        {
            Parallel_For();
        }
        catch (AggregateException)
        {
            // handle exception
        }

        try
        {
            Parallel_ForEach();
        }
        catch (AggregateException)
        {
            // handle exception
        }

        try
        {
            Parallel_Invoke();
        }
        catch (AggregateException)
        {
            // handle exception
        }
    }

    private void Parallel_For()
    {
        // work to be done
        int[] array = Enumerable.Range(start: 1, count: 100).ToArray();

        int sum = 0;

        // synchronous for loop
        // for (int i = 0; i < array.Length; i++)
        // {
        //     sum += array[i];
        // }

        // blocking for parallel loop
        ParallelLoopResult result = Parallel.For(
            fromInclusive: 0,
            toExclusive: array.Length,
            body: (int index, ParallelLoopState parallelLoopState) =>
        {
            lock (_lock)
            {
                // only if no exception has been thrown from any thread
                if (!parallelLoopState.IsExceptional)
                {
                    if (index == 65)
                    {
                        throw new InvalidOperationException("Something went wrong");
                    }

                    sum += array[index];
                    Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
                }
            }
        });

        Console.WriteLine($"The sum is {sum}");
    }

    private void Parallel_ForEach()
    {
        // work to be done
        int[] array = Enumerable.Range(start: 1, count: 100).ToArray();

        int sum = 0;

        // synchronous for each loop
        // foreach (var item in array)
        // {
        //     sum += item;
        // }

        // blocking for parallel loop
        // does not use 10 threads for 10 elements but uses a thread pool
        ParallelLoopResult result = Parallel.ForEach(
            source: array,
            body: (int item, ParallelLoopState parallelLoopState) =>
        {
            lock (_lock)
            {
                // only if no exception has been thrown from any thread
                if (!parallelLoopState.IsExceptional)
                {
                    if (item == 65)
                    {
                        throw new InvalidOperationException("Something went wrong");
                    }

                    sum += item;
                    Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
                }
            }
        });

        Console.WriteLine($"The sum is {sum}");
    }

    private void Parallel_Invoke()
    {
        // blocking parallel invoke
        Parallel.Invoke(actions: [
            () => Console.WriteLine("Action 1"),
            () => Console.WriteLine("Action 2"),
            () => Console.WriteLine("Action 3")
        ]);
    }
}