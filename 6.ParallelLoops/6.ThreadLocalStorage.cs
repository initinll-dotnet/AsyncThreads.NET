namespace ParallelLoops;

public class ThreadLocalStorage
{
    private readonly Lock _lock = new Lock();

    public void Execute()
    {
        int[] array = Enumerable.Range(1, 10).ToArray();

        int sum = 0;

        Parallel.For(
            fromInclusive: 0,
            toExclusive: array.Length,
            localInit: () => 0,
            body: (int index, ParallelLoopState parallelLoopState, int threadlocalstorage_subtotal) =>
            {
                // summation per thread and storing in thread local storage variable
                threadlocalstorage_subtotal += array[index];
                Console.WriteLine($"Task {Task.CurrentId} has subtotal {threadlocalstorage_subtotal}");
                return threadlocalstorage_subtotal;
            },
            localFinally: (int threadlocalstorage_subtotal) =>
            {
                // summation of all thread local storage variables
                lock (_lock)
                {
                    Console.WriteLine($"Local finally subtotal for task {Task.CurrentId} is {threadlocalstorage_subtotal}");
                }
            });
    }
}