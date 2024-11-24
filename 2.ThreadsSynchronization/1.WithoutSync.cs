namespace ThreadsSynchronization;

public class WithoutSync
{
    private int counter = 0;

    public void Execute()
    {
        Thread thread1 = new Thread(IncrementCounter);
        Thread thread2 = new Thread(IncrementCounter);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine($"Final counter value is: {counter}");
    }

    private void IncrementCounter()
    {
        for (int i = 0; i < 100000; i++)
        {
            counter = counter + 1;
        }
    }
}
