using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CodeFactory.Database_Deadlock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeadlockController : ControllerBase
{
    // These semaphores act as "locks" representing database tables.
    // Initialized with 1, meaning only one request can access the resource at a time.
    private static readonly SemaphoreSlim StudentTableLock = new SemaphoreSlim(1, 1);
    private static readonly SemaphoreSlim CourseTableLock = new SemaphoreSlim(1, 1);

    private readonly ILogger<DeadlockController> _logger;

    // Inject ASP.NET Core Logger to output logs to the terminal/debug window
    public DeadlockController(ILogger<DeadlockController> logger)
    {
        _logger = logger;
    }

    // Case 1: Locks Student table first, then tries to lock Course table
    [HttpGet("case1")]
    public async Task<IActionResult> GetCase1()
    {
        _logger.LogInformation("Case 1: Request started. Attempting to lock Student table...");

        bool hasStudentLock = false;
        bool hasCourseLock = false;

        try
        {
            // Acquire the Student lock
            await StudentTableLock.WaitAsync();
            hasStudentLock = true;
            _logger.LogInformation("Case 1: Student table is locked. Simulating work (waiting 5 seconds)...");
            
            // Wait 5 seconds to give Case 2 time to lock the Course table
            await Task.Delay(5000);

            _logger.LogInformation("Case 1: Attempting to lock Course table (will wait indefinitely if Case 2 is running)...");
            
            // Try to acquire the Course lock (which is held by Case 2)
            await CourseTableLock.WaitAsync();
            hasCourseLock = true;

            _logger.LogInformation("Case 1: Course table is locked. Completed successfully.");
        }
        finally
        {
            // Release the Course lock if we acquired it and it hasn't been reset (CurrentCount is 0)
            if (hasCourseLock && CourseTableLock.CurrentCount == 0)
            {
                CourseTableLock.Release();
            }

            // Release the Student lock if we acquired it and it hasn't been reset (CurrentCount is 0)
            if (hasStudentLock && StudentTableLock.CurrentCount == 0)
            {
                StudentTableLock.Release();
            }
        }

        return Ok("Case 1 completed successfully.");
    }

    // Case 2: Locks Course table first, then tries to lock Student table
    [HttpGet("case2")]
    public async Task<IActionResult> GetCase2()
    {
        _logger.LogInformation("Case 2: Request started. Attempting to lock Course table...");

        bool hasStudentLock = false;
        bool hasCourseLock = false;

        try
        {
            // Acquire the Course lock
            await CourseTableLock.WaitAsync();
            hasCourseLock = true;
            _logger.LogInformation("Case 2: Course table is locked. Simulating work (waiting 5 seconds)...");
            
            // Wait 5 seconds to give Case 1 time to lock the Student table
            await Task.Delay(5000);

            _logger.LogInformation("Case 2: Attempting to lock Student table (will wait indefinitely if Case 1 is running)...");
            
            // Try to acquire the Student lock (which is held by Case 1)
            await StudentTableLock.WaitAsync();
            hasStudentLock = true;

            _logger.LogInformation("Case 2: Student table is locked. Completed successfully.");
        }
        finally
        {
            // Release the Student lock if we acquired it and it hasn't been reset (CurrentCount is 0)
            if (hasStudentLock && StudentTableLock.CurrentCount == 0)
            {
                StudentTableLock.Release();
            }

            // Release the Course lock if we acquired it and it hasn't been reset (CurrentCount is 0)
            if (hasCourseLock && CourseTableLock.CurrentCount == 0)
            {
                CourseTableLock.Release();
            }
        }

        return Ok("Case 2 completed successfully.");
    }

    // Reset Endpoint: Releases locks manually if the server gets frozen/deadlocked
    [HttpGet("reset")]
    public IActionResult Reset()
    {
        _logger.LogInformation("Reset: Releasing locks...");

        // Safely release Student lock if it is currently locked
        if (StudentTableLock.CurrentCount == 0)
        {
            StudentTableLock.Release();
        }

        // Safely release Course lock if it is currently locked
        if (CourseTableLock.CurrentCount == 0)
        {
            CourseTableLock.Release();
        }

        _logger.LogInformation("Reset: All locks have been reset.");
        return Ok("Locks reset successfully. Blocked endpoints will now unfreeze and complete.");
    }
}
