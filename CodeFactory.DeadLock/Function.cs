using System.Transactions;

namespace CodeFactory.DeadLock;

public  class Function
{
    private static readonly object LockA = new object();
    private static readonly object LockB = new object();

    public  static String Transaction()
    {
        Console.WriteLine("Transaction started");
        Thread.Sleep(1000); // Simulate some work
        Console.WriteLine("Transaction completed");
        return "Transaction completed";
    }

    public static String Billing()
    {
        Console.WriteLine("Billing started");
        Thread.Sleep(1000); // Simulate some work
        Console.WriteLine("Billing completed");
        return "Billing completed";
    }

    public async static Task<String> TransactionAsync()
    {
        Console.WriteLine("Transaction started");
        await Task.Delay(1000); // Simulate some asynchronous work
        Console.WriteLine("Transaction completed");
        return "Transaction completed";
    }

    public async static Task<String> BillingAsync()
    {
        Console.WriteLine("Billing started");
        await Task.Delay(1000); // Simulate some asynchronous work
        Console.WriteLine("Billing completed");
        return "Billing completed";
    }


    public static void TriggerDeadlock()
    {
        Console.WriteLine("Triggering deadlock scenario...");

        Task task1 = null!;
        Task task2 = null!;

        task1 = Task.Run(() => task2.Wait()); // Task 1 waits for Task 2
        task2 = Task.Run(() => task1.Wait()); // Task 2 waits for Task 1

        task1.Wait(); // Blocks the main thread forever because Task 1 is waiting on Task 2
    }
}
