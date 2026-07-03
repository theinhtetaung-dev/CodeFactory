using System;
using System.Collections.Generic;

namespace CodeFactory.Database_Deadlock.Models;

public class StudentCourseSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
}

public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public List<StudentCourseSummaryDto> Courses { get; set; } = new();
}

// Get All
public class GetStudentsRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetStudentsResponseDto
{
    public List<StudentDto> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

// Get By Id
public class GetStudentByIdResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public List<StudentCourseSummaryDto> Courses { get; set; } = new();
}

// Create
public class CreateStudentRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<int> CourseIds { get; set; } = new();
}

public class CreateStudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public List<int> CourseIds { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}

// Update
public class UpdateStudentRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<int> CourseIds { get; set; } = new();
}

public class UpdateStudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public List<int> CourseIds { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}

// Delete
public class DeleteStudentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
