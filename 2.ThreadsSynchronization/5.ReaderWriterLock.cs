namespace ThreadsSynchronization;

public class ReaderWriterLock
{
    private readonly ReaderWriterLockSlim _lock = new();
    private Dictionary<int, string> _cache = new();

    public async Task Execute()
    {
        var t1 = Task.Run(() => Add(1, "One"));
        var t2 = Task.Run(() => Add(2, "Two"));
        var t3 = Task.Run(() => Add(3, "Three"));
        var t4 = Task.Run(() => Add(4, "Four"));
        var t5 = Task.Run(() => Add(5, "Five"));

        var t6 = Task.Run(() => Console.WriteLine(Get(1)));
        var t7 = Task.Run(() => Console.WriteLine(Get(2)));
        var t8 = Task.Run(() => Console.WriteLine(Get(3)));
        var t9 = Task.Run(() => Console.WriteLine(Get(4)));
        var t10 = Task.Run(() => Console.WriteLine(Get(5)));

        await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
    }

    private void Add(int key, string value)
    {
        bool lockAcquired = false;
        try
        {
            _lock.EnterWriteLock();
            lockAcquired = true;
            _cache[key] = value;
        }
        finally
        {
            if (lockAcquired) _lock.ExitWriteLock();
        }
    }

    private string? Get(int key)
    {
        bool lockAcquired = false;
        try
        {
            _lock.EnterReadLock();
            lockAcquired = true;
            return _cache.TryGetValue(key, out var value) ? value : null;
        }
        finally
        {
            if (lockAcquired) _lock.ExitReadLock();
        }
    }
}
