using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFactory.Database_Deadlock.Models;

[Table("Tbl_Student")]
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }

    // Navigation property for many-to-many relationship
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
