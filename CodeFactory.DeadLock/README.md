# Deadlock Simulation and Prevention

This project demonstrates synchronous vs. asynchronous method execution in C#, compares parallel asynchronous task execution, and simulates a classic deadlock scenario to understand how concurrency issues occur and how to prevent them.

---

## 1. What is a Deadlock?

A **deadlock** is a concurrency situation where two or more threads (or tasks) are blocked forever, each waiting for the other to release a resource, lock, or complete an action. Because none of the threads can proceed, the entire application or component freezes.

Historically, a deadlock requires four conditions to occur simultaneously (known as the **Coffman Conditions**):
1. **Mutual Exclusion:** At least one resource must be held in a non-shareable mode (only one thread can use it at a time).
2. **Hold and Wait:** A thread holding resources can request additional resources without relinquishing its current ones.
3. **No Preemption:** Resources cannot be forcibly taken from a thread holding them; they must be released voluntarily.
4. **Circular Wait:** A closed chain of threads exists, where each thread holds one or more resources needed by the next thread in the chain.

---

## 2. How This Program Simulates a Deadlock

In, the method `TriggerDeadlock()` simulates a deadlock using task-to-task dependency:

```csharp
public static void TriggerDeadlock()
{
    Console.WriteLine("Triggering deadlock scenario...");

    Task task1 = null!;
    Task task2 = null!;

    task1 = Task.Run(() => task2.Wait()); // Task 1 waits for Task 2
    task2 = Task.Run(() => task1.Wait()); // Task 2 waits for Task 1

    task1.Wait(); // Blocks the main thread forever because Task 1 is waiting on Task 2
}
```

### Step-by-Step Execution Flow:
1. **Instantiation:** `task1` and `task2` are initialized as null tasks.
2. **Task 1 Scheduling:** `task1` is queued to run on the ThreadPool. Inside its body, it calls `task2.Wait()`. This means `task1` will block and wait for `task2` to complete.
3. **Task 2 Scheduling:** `task2` is queued to run on the ThreadPool. Inside its body, it calls `task1.Wait()`. This means `task2` will block and wait for `task1` to complete.
4. **Circular Block:** 
   - `task1` cannot complete until `task2` completes.
   - `task2` cannot complete until `task1` completes.
   - This creates a direct **Circular Wait** dependency.
5. **Main Thread Block:** The main thread executes `task1.Wait()`, blocking itself indefinitely as well. The program freezes and never reaches the "Press any key to exit..." output.

---

## 3. Rules to Obey to Avoid Deadlocks

To write safe concurrent and parallel code, you must obey the following guidelines and best practices:

### 1. Avoid Blocking on Async Code (No Sync-over-Async)
Never use `.Wait()`, `.Result`, or `.GetAwaiter().GetResult()` on a `Task` when calling asynchronous methods from synchronous code.
* **Why:** In some environments (like desktop or legacy ASP.NET apps), this blocks the single UI/request thread. When the task finishes, it tries to resume on that same thread, causing a deadlock.
* **Solution:** Go "async all the way." Use `await` instead of blocking methods.

### 2. Lock Ordering (Consistent Lock Acquisition)
If your code needs to acquire multiple locks, always acquire them in the exact same order across all threads.
* **Bad:** 
  - Thread 1: Locks `LockA` then locks `LockB`.
  - Thread 2: Locks `LockB` then locks `LockA`. (Deadlock risk!)
* **Good:**
  - Both Thread 1 and Thread 2: Lock `LockA` first, then lock `LockB`.

### 3. Use Timeout-based Locking
Avoid waiting indefinitely for a lock or task completion.
* **Locking:** Use `Monitor.TryEnter` with a timeout instead of the `lock` statement.
* **Tasks:** Use a cancellation token (`CancellationToken`) or helper methods like `Task.WhenAny` with a timeout task (e.g., `Task.Delay(timeout)`) so that your code can recover and report an error if a deadlock or slow operation occurs.

### 4. Keep Lock Scope Small and Clean
Only lock the critical section of code that modifies shared state.
* Do not perform long-running operations (like I/O, database queries, or network requests) while holding a lock.
* Never call external or untrusted code (such as events or callbacks) from within a lock, as you don't know what locks that external code might attempt to acquire.

### 5. Use Modern Thread-Safe Collections and Constructs
Instead of manual locking, use high-level abstractions:
* **Concurrent Collections:** `ConcurrentDictionary`, `ConcurrentQueue`, etc.
* **Async-Safe Synchronization:** Use `SemaphoreSlim` instead of `lock` if you need synchronization across `await` boundaries (standard `lock` blocks cannot contain `await`).
