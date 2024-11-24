namespace ThreadIntroduction;

public class DivideAndConquer
{
    public void Execute()
    {
        // work to be done
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;

        var startTime = DateTime.Now;

        int numofThreads = 4;
        int segmentLength = array.Length / numofThreads;

        // diving the array into 4 segments and calculating the sum of each segment
        Thread[] threads = new Thread[numofThreads];
        threads[0] = new Thread(() => { sum1 = SumSegment(0, segmentLength, array); });
        threads[1] = new Thread(() => { sum2 = SumSegment(segmentLength, 2 * segmentLength, array); });
        threads[2] = new Thread(() => { sum3 = SumSegment(2 * segmentLength, 3 * segmentLength, array); });
        threads[3] = new Thread(() => { sum4 = SumSegment(3 * segmentLength, array.Length, array); });

        // starting the threads
        foreach (var thread in threads) { thread.Start(); }

        // waiting for all threads to complete
        foreach (var thread in threads) { thread.Join(); }

        var endTime = DateTime.Now;

        var timespan = endTime - startTime;

        Console.WriteLine($"The sum is {sum1 + sum2 + sum3 + sum4}");
        Console.WriteLine($"The time it takes: {timespan.TotalMilliseconds}");

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
