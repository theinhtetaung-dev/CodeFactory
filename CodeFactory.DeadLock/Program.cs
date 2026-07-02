// See https://aka.ms/new-console-template for more information
using CodeFactory.DeadLock;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
#region Synchronous Methods
Stopwatch sw = Stopwatch.StartNew();
String result = Function.Transaction();
String result2 = Function.Billing();
sw.Stop();
Console.WriteLine("Stopwatch elapsed time: " + sw.ElapsedMilliseconds + " ms");
Console.WriteLine("---------------------");
#endregion

#region Asynchronous Methods
sw.Restart();
String result3 = await Function.TransactionAsync();
String result4 = await Function.BillingAsync();
sw.Stop();
Console.WriteLine("Stopwatch elapsed time for async methods: " + sw.ElapsedMilliseconds + " ms");
Console.WriteLine("---------------------");
#endregion

#region Parallel Asynchronous Methods
sw.Restart();
Task<string> task1 = Function.TransactionAsync();
Task<string> task2 = Function.BillingAsync();
await Task.WhenAll(task1, task2);
sw.Stop();
Console.WriteLine("Stopwatch elapsed time for parallel async methods: " + sw.ElapsedMilliseconds + " ms");
Console.WriteLine("---------------------");
#endregion


Function.TriggerDeadlock(); //creating a deadlock scenario

Console.WriteLine("Press any key to exit...");
Console.Read();