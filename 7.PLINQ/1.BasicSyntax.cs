namespace PLINQ;

public class BasicSyntax
{
    public void Execute()
    {
        var items = Enumerable.Range(1, 200);

        ParallelQuery<int> evenNumbers = items.AsParallel().Where(x =>
        {
            Console.WriteLine($"Processing number {x}; Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            return (x % 2 == 0);
        });

        //Console.WriteLine($"There are {evenNumbers.Count()} even numbers in the collection.");

        // foreach - results are merged and run on the main thread
        //foreach (var item in evenNumbers)
        //{
        //    Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        //}

        // ForAll - results are not merged and run on multiple threads
        evenNumbers.ForAll(item =>
        {
            Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        });
    }
}