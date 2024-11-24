namespace ParallelLoops;

public class BasicSyntax
{
    private readonly Lock _lock = new();

    public void Execute()
    {
        // all three methods Parallel.For, Parallel.ForEach & Parallel.Invoke are blocking and synchronous
        Parallel_For();
        Parallel_ForEach();
        Parallel_Invoke();
    }

    private void Parallel_For()
    {
        // work to be done
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

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
            body: index =>
        {
            lock (_lock)
            {
                sum += array[index];
                Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
            }
        });

        Console.WriteLine($"The sum is {sum}");
    }

    private void Parallel_ForEach()
    {
        // work to be done
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        int sum = 0;

        // synchronous for each loop
        // foreach (var item in array)
        // {
        //     sum += item;
        // }

        // blocking for parallel loop
        // does not use 10 threads for 10 elements but uses a thread pool
        ParallelLoopResult result = Parallel.ForEach(source: array, body: item =>
        {
            lock (_lock)
            {
                sum += item;
                Console.WriteLine($"Current Task Id: {Task.CurrentId}; Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
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