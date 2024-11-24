using System.Collections.Concurrent;

namespace ThreadsSynchronization;

public class SeatBookingSystem
{
    private int availableTickets = 10;
    // thread safe queue
    private ConcurrentQueue<string?> requestQueue = new();
    private readonly Lock myLock = new();


    public void Execute()
    {
        // main thread

        // 2. Start the requests queue monitoring thread
        Thread monitoringThread = new Thread(MonitorQueue);
        monitoringThread.Start();

        // 1. Enqueue the requests
        Console.WriteLine("Server is running. \r\n Type 'b' to book a ticket. \r\n Type 'c' to cancel. \r\n Type 'exit' to stop. \r\n");

        while (true)
        {
            string? input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
                break;
            }

            requestQueue.Enqueue(input);
        }
    }

    // runs in a separate thread
    private void MonitorQueue()
    {
        while (true)
        {
            if (requestQueue.Count > 0)
            {
                // processing done in a separate thread
                var result = requestQueue.TryDequeue(out string? input) ? input : null;
                Thread processingThread = new Thread(() => ProcessBooking(input));
                processingThread.Start();
            }
            Thread.Sleep(100);
        }
    }

    // 3. Processing the requests
    private void ProcessBooking(string? input)
    {
        // locking with timeout
        if (myLock.TryEnter(TimeSpan.FromSeconds(2)))
        {
            // Simulate processing time
            Thread.Sleep(10_000);

            try
            {
                Console.WriteLine("Main thread acquired the lock");
                BookOrCancelSeat(input);
            }
            finally
            {
                myLock.Exit();
                Console.WriteLine("Main thread released the lock");
            }
        }
        else
        {
            Console.WriteLine("System is busy. Please try again later.");
        }

        // locking without timeout
        // lock (myLock)
        // {
        //     BookOrCancelSeat(input);
        // }
    }

    private void BookOrCancelSeat(string input)
    {
        if (input == "b")
        {
            if (availableTickets > 0)
            {
                availableTickets--;
                Console.WriteLine();
                Console.WriteLine($"Your seat is booked. {availableTickets} seats are still available.");
            }
            else
            {
                Console.WriteLine($"Tickets are not available.");
            }
        }
        else if (input == "c")
        {
            if (availableTickets < 10)
            {
                availableTickets++;
                Console.WriteLine();
                Console.WriteLine($"Your booking is canceled. {availableTickets} seats are available.");
            }
            else
            {
                Console.WriteLine($"Error. You cannot cancel a booking at this time.");
            }
        }
    }
}
