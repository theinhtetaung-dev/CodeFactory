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
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/courses?PageNumber=1&PageSize=10
    [HttpGet]
    public async Task<ActionResult<GetCoursesResponseDto>> GetAll([FromQuery] GetCoursesRequestDto request)
    {
        if (request.PageNumber < 1) request.PageNumber = 1;
        if (request.PageSize < 1) request.PageSize = 10;

        var query = _context.Courses
            .Include(c => c.Students)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Credits = c.Credits,
                Students = c.Students.Select(s => new CourseStudentSummaryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email
                }).ToList()
            })
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var response = new GetCoursesResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Ok(response);
    }

    // GET: api/courses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCourseByIdResponseDto>> GetById(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Students)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            return NotFound(new { Message = $"Course with ID {id} not found." });
        }

        var response = new GetCourseByIdResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            Students = course.Students.Select(s => new CourseStudentSummaryDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList()
        };

        return Ok(response);
    }

    // POST: api/courses
    [HttpPost]
    public async Task<ActionResult<CreateCourseResponseDto>> Create([FromBody] CreateCourseRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { Message = "Course title is required." });
        }

        var course = new Course
        {
            Title = request.Title,
            Credits = request.Credits
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var response = new CreateCourseResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            Message = "Course created successfully."
        };

        return CreatedAtAction(nameof(GetById), new { id = course.Id }, response);
    }

    // PUT: api/courses/5
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateCourseResponseDto>> Update(int id, [FromBody] UpdateCourseRequestDto request)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound(new { Message = $"Course with ID {id} not found." });
        }

        course.Title = request.Title;
        course.Credits = request.Credits;

        await _context.SaveChangesAsync();

        var response = new UpdateCourseResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            Message = "Course updated successfully."
        };

        return Ok(response);
    }

    // DELETE: api/courses/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteCourseResponseDto>> Delete(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound(new { Message = $"Course with ID {id} not found." });
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        var response = new DeleteCourseResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            Message = "Course deleted successfully."
        };

        return Ok(response);
    }
}
