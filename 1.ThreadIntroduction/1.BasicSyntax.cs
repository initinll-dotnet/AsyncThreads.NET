namespace ThreadIntroduction;

public class BasicSyntax
{
    // threads are non blocking in nature
    // main thread is blocking in nature
    public void Execute()
    {
        Thread thread1 = new Thread(WriteThreadId);
        Thread thread2 = new Thread(WriteThreadId);

        thread1.Name = "Thread1";
        thread2.Name = "Thread2";
        Thread.CurrentThread.Name = "Main thread";

        thread1.Priority = ThreadPriority.Highest;
        thread2.Priority = ThreadPriority.Lowest;
        Thread.CurrentThread.Priority = ThreadPriority.Normal;

        // thread scheduler will schedule the threads on cpu
        // no guarantee of order of execution nd no control over scheduler, it is managed by OS
        thread1.Start();
        thread2.Start();

        WriteThreadId();

        Console.ReadLine();
    }

    private void WriteThreadId()
    {
        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine(Thread.CurrentThread.Name);
            //Thread.Sleep(50);
        }
    }
}
