namespace ParallelLoops;

public class BreakingLoop
{
    private readonly Lock _lock = new();

    public void Execute()
    {
        // After calling state.Break(), the loop completes iterations up to the current iteration index.
        // Iterations with an index lower than the current iteration continue, but higher indices do not start.
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
                // ShouldExitCurrentIteration is a property that can be used to check if a request to stop the loop has been made
                // Indicates whether the current iteration should exit prematurely

                // LowestBreakIteration is a property that can be used to check the index of the iteration that requested the stoppage of the loop
                // Indicates the lowest iteration index for which Break() has been called.
                if (
                    parallelLoopState.ShouldExitCurrentIteration &&
                    parallelLoopState.LowestBreakIteration < index)
                {
                    return;
                }

                // only if no stoppage has been requested from any thread
                if (index == 65)
                {
                    // request to stop the loop
                    parallelLoopState.Break();
                }

                sum += array[index];
                Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
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
            body: (int item, ParallelLoopState parallelLoopState, long index) =>
        {
            // ShouldExitCurrentIteration is a property that can be used to check if a request to stop the loop has been made
            // Indicates whether the current iteration should exit prematurely

            // LowestBreakIteration is a property that can be used to check the index of the iteration that requested the stoppage of the loop
            // Indicates the lowest iteration index for which Break() has been called.
            lock (_lock)
            {
                if (
                    parallelLoopState.ShouldExitCurrentIteration &&
                    parallelLoopState.LowestBreakIteration < index)
                {
                    return;
                }

                if (item == 65)
                {
                    // request to stop the loop
                    parallelLoopState.Break();
                }

                sum += item;
                Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
            }
        });

        Console.WriteLine($"The sum is {sum}");
    }
}