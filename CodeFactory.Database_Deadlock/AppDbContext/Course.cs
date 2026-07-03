using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFactory.Database_Deadlock.Models;

[Table("Tbl_Course")]
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }

    // Navigation property for many-to-many relationship
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
