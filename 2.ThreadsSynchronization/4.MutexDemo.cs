namespace ThreadsSynchronization;

public class MutexDemo
{
    public void Execute()
    {
        string filePath = "counter.txt";

        using (var mutex = new Mutex(initiallyOwned: false, name: $"GlobalFileMutex:{filePath}"))
        {
            for (int i = 0; i < 10_000; i++)
            {
                mutex.WaitOne();
                try
                {
                    int counter = ReadCounter(filePath);
                    counter++;
                    WriteCounter(filePath, counter);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        Console.WriteLine("Process finished.");
        Console.ReadLine();
    }


    private int ReadCounter(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
        {
            string content = reader.ReadToEnd();
            return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
        }
    }

    private void WriteCounter(string filePath, int counter)
    {
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(counter);
        }
    }
}
