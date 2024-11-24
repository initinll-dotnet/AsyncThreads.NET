namespace ThreadsSynchronization;

public class ProducerConsumerDemo
{
    Queue<int> queue = new();

    ManualResetEventSlim consumeEvent = new(initialState: false);
    ManualResetEventSlim produceEvent = new(initialState: true);

    private int consumerCount = 0;
    private readonly Lock lockConsumerCount = new();

    private Thread[] consumerThreads = new Thread[3];

    ~ProducerConsumerDemo()
    {
        consumeEvent.Dispose();
        produceEvent.Dispose();
    }

    public void Execute()
    {
        // Start the worker thread
        StartWorkerThreads();

        StartProducer();
        StartConsumer();

        // Exit the application
        Environment.Exit(0);
    }

    private void StartWorkerThreads()
    {
        // Start the worker thread
        for (int i = 0; i < 3; i++)
        {
            consumerThreads[i] = new Thread(StartConsumer);
            consumerThreads[i].Name = $"Consumer {i + 1}";
            consumerThreads[i].Start();
        }
    }

    private void StartProducer()
    {
        while (true)
        {
            // wait for the signal to produce
            produceEvent.Wait();

            // make producer block after the below batch is produced
            // consumer will signal when it is ready to consume to producer
            produceEvent.Reset();

            Console.WriteLine("To produce, enter 'p' or 'e' to exit.");
            var input = Console.ReadLine() ?? "";

            if (input.ToLower() == "p")
            {
                // produce a batch of 10 items
                for (int i = 1; i <= 10; i++)
                {
                    queue.Enqueue(i);
                    Console.WriteLine($"Produced: {i}");
                }

                // signal to consumer threads that items are ready
                consumeEvent.Set();
            }

            if (input.ToLower() == "e")
            {
                Console.WriteLine("Exited ..");
                Environment.Exit(0);
            }
        }
    }

    // Consumer's behavior
    private void StartConsumer()
    {
        while (true)
        {
            // wait for the signal to consume
            consumeEvent.Wait();

            // post signal, consume the items
            while (queue.TryDequeue(out int item))
            {
                // work on the items produced
                Thread.Sleep(500);
                Console.WriteLine($"Consumed: {item} from thread: {Thread.CurrentThread.Name}");
            }

            // there are 3 threads consuming the items
            // increase the consumer count via lock to make it thread-safe
            lock (lockConsumerCount)
            {
                consumerCount++;

                // check if consumer count is 3
                if (consumerCount == 3)
                {
                    // signal to consumer threads to wait for next batch
                    consumeEvent.Reset();

                    // signal to producer that it may now produce
                    produceEvent.Set();

                    // resetting consumer count to 0
                    consumerCount = 0;

                    Console.WriteLine("****************");
                    Console.WriteLine("**** More Please! *****");
                    Console.WriteLine("****************");
                }
            }
        }
    }
}
