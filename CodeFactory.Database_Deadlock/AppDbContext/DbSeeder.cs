using CodeFactory.Database_Deadlock.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFactory.Database_Deadlock.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Automatically apply EF migrations on startup
        context.Database.Migrate();

        // Check if DB is already seeded
        if (context.Students.Any() || context.Courses.Any())
        {
            return;
        }

        // Create sample courses
        var courseCS = new Course { Title = "Computer Science 101", Credits = 4 };
        var courseDB = new Course { Title = "Introduction to Databases", Credits = 3 };
        var courseSE = new Course { Title = "Software Engineering Principles", Credits = 3 };
        var courseAI = new Course { Title = "Artificial Intelligence", Credits = 4 };

        context.Courses.AddRange(courseCS, courseDB, courseSE, courseAI);

        // Create sample students
        var studentAlice = new Student
        {
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            EnrollmentDate = DateTime.UtcNow.AddMonths(-10)
        };

        var studentBob = new Student
        {
            Name = "Bob Smith",
            Email = "bob.smith@example.com",
            EnrollmentDate = DateTime.UtcNow.AddMonths(-6)
        };

        var studentCharlie = new Student
        {
            Name = "Charlie Brown",
            Email = "charlie.brown@example.com",
            EnrollmentDate = DateTime.UtcNow.AddMonths(-2)
        };

        // Establish student-course relationships
        studentAlice.Courses.Add(courseCS);
        studentAlice.Courses.Add(courseDB);

        studentBob.Courses.Add(courseDB);
        studentBob.Courses.Add(courseSE);

        studentCharlie.Courses.Add(courseCS);
        studentCharlie.Courses.Add(courseAI);

        context.Students.AddRange(studentAlice, studentBob, studentCharlie);

        // Save changes to database
        context.SaveChanges();
    }
}
