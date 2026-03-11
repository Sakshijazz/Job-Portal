using JobPortal_Backend.Data;
using JobPortal_Backend.DTOs;
using JobPortal_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // Apply for job
        [Authorize(Roles = "User")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyJob(ApplyJobDto dto)
        {
            // check if job exists
            var job = await _context.Jobs.FindAsync(dto.JobId);
            if (job == null)
                return NotFound("Job not found");

            // check if already applied
            var existingApplication = await _context.Applications
                .FirstOrDefaultAsync(a => a.JobId == dto.JobId && a.UserId == dto.UserId);

            if (existingApplication != null)
                return BadRequest("You already applied for this job");

            var application = new Application
            {
                JobId = dto.JobId,
                UserId = dto.UserId,
                ApplicationDate = DateTime.Now,
                Status = "Applied"
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return Ok("Application submitted successfully");
        }

        // View applications of logged user
        [HttpGet("my-applications")]
        public async Task<ActionResult<IEnumerable<Application>>> MyApplications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var applications = await _context.Applications
                .Where(a => a.UserId == userId)
                .Include(a => a.JobId)
                .ToListAsync();

            return applications;
        }

        // Admin: view all applications
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications

                .Include(a => a.JobId)
                .ToListAsync();
        }

        // Get application by ID
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplicationById(int id)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (application == null)
                return NotFound("Application not found");

            return Ok(application);
        }

        // Admin: update application status
        [Authorize(Roles = "Admin")]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, UpdateApplicationStatusDto dto)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
                return NotFound();

            application.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(application);
        }

        // Delete application
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
                return NotFound();

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}