using CodeFactory.Database_Deadlock.Data;
using CodeFactory.Database_Deadlock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFactory.Database_Deadlock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/students?PageNumber=1&PageSize=10
    [HttpGet]
    public async Task<ActionResult<GetStudentsResponseDto>> GetAll([FromQuery] GetStudentsRequestDto request)
    {
        if (request.PageNumber < 1) request.PageNumber = 1;
        if (request.PageSize < 1) request.PageSize = 10;

        var query = _context.Students
            .Include(s => s.Courses)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                EnrollmentDate = s.EnrollmentDate,
                Courses = s.Courses.Select(c => new StudentCourseSummaryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits
                }).ToList()
            })
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var response = new GetStudentsResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Ok(response);
    }

    // GET: api/students/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetStudentByIdResponseDto>> GetById(int id)
    {
        var student = await _context.Students
            .Include(s => s.Courses)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return NotFound(new { Message = $"Student with ID {id} not found." });
        }

        var response = new GetStudentByIdResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            EnrollmentDate = student.EnrollmentDate,
            Courses = student.Courses.Select(c => new StudentCourseSummaryDto
            {
                Id = c.Id,
                Title = c.Title,
                Credits = c.Credits
            }).ToList()
        };

        return Ok(response);
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<CreateStudentResponseDto>> Create([FromBody] CreateStudentRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { Message = "Student name is required." });
        }

        var student = new Student
        {
            Name = request.Name,
            Email = request.Email,
            EnrollmentDate = DateTime.UtcNow
        };

        if (request.CourseIds != null && request.CourseIds.Count > 0)
        {
            var courses = await _context.Courses
                .Where(c => request.CourseIds.Contains(c.Id))
                .ToListAsync();

            foreach (var course in courses)
            {
                student.Courses.Add(course);
            }
        }

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        var response = new CreateStudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            EnrollmentDate = student.EnrollmentDate,
            CourseIds = student.Courses.Select(c => c.Id).ToList(),
            Message = "Student created successfully."
        };

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, response);
    }

    // PUT: api/students/5
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateStudentResponseDto>> Update(int id, [FromBody] UpdateStudentRequestDto request)
    {
        var student = await _context.Students
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return NotFound(new { Message = $"Student with ID {id} not found." });
        }

        student.Name = request.Name;
        student.Email = request.Email;

        student.Courses.Clear();
        if (request.CourseIds != null && request.CourseIds.Count > 0)
        {
            var courses = await _context.Courses
                .Where(c => request.CourseIds.Contains(c.Id))
                .ToListAsync();

            foreach (var course in courses)
            {
                student.Courses.Add(course);
            }
        }

        await _context.SaveChangesAsync();

        var response = new UpdateStudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            EnrollmentDate = student.EnrollmentDate,
            CourseIds = student.Courses.Select(c => c.Id).ToList(),
            Message = "Student updated successfully."
        };

        return Ok(response);
    }

    // DELETE: api/students/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteStudentResponseDto>> Delete(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound(new { Message = $"Student with ID {id} not found." });
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        var response = new DeleteStudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Message = "Student deleted successfully."
        };

        return Ok(response);
    }
}
