namespace PLINQ;

public class ProducerConsumerBuffer
{
    public void Execute()
    {
        var items = Enumerable.Range(1, 200);

        // producer
        ParallelQuery<int> evenNumbers = items
        .AsParallel()
        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
        .Where(x =>
        {
            Console.WriteLine($"Processing number {x}; Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            return (x % 2 == 0);
        });

        Console.WriteLine();
        //Console.WriteLine($"There are {evenNumbers.Count()} even numbers in the collection.");

        // consumer
        foreach (var item in evenNumbers)
        {
            Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}