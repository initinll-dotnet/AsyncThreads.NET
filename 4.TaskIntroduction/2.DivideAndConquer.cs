namespace TaskIntroduction;

public class DivideAndConquer
{
    public void Execute()
    {
        // work to be done
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var startTime = DateTime.Now;

        int numofThreads = 4;
        int segmentLength = array.Length / numofThreads;

        Task<int>[] tasks =
        [
            Task.Run(() => SumSegment(0, segmentLength, array)),
            Task.Run(() => SumSegment(segmentLength, 2 * segmentLength, array)),
            Task.Run(() => SumSegment(2 * segmentLength, 3 * segmentLength, array)),
            Task.Run(() => SumSegment(3 * segmentLength, array.Length, array))
        ];

        // non blocking
        Task.WhenAll(tasks).ContinueWith(t =>
        {
            Console.WriteLine($"The summary is {t.Result.Sum()}");
        });

        Console.WriteLine("This is the end of the program.");

        Console.ReadLine();
    }

    private int SumSegment(int start, int end, int[] array)
    {
        int segmentSum = 0;
        for (int i = start; i < end; i++)
        {
            Thread.Sleep(100);
            segmentSum += array[i];
        }
        return segmentSum;
    }
}
