using System.Collections.Generic;

namespace CodeFactory.Database_Deadlock.Models;

public class CourseStudentSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public List<CourseStudentSummaryDto> Students { get; set; } = new();
}

// Get All
public class GetCoursesRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetCoursesResponseDto
{
    public List<CourseDto> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

// Get By Id
public class GetCourseByIdResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public List<CourseStudentSummaryDto> Students { get; set; } = new();
}

// Create
public class CreateCourseRequestDto
{
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
}

public class CreateCourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string Message { get; set; } = string.Empty;
}

// Update
public class UpdateCourseRequestDto
{
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
}

public class UpdateCourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string Message { get; set; } = string.Empty;
}

// Delete
public class DeleteCourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
