using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobPortal_Backend.Data;
using JobPortal_Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JobPortal_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Dashboard statistics
        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardDto>> GetDashboardStats()
        {
            var totalJobs = await _context.Jobs.CountAsync();
            var totalApplicants = await _context.Users.CountAsync(u => u.Role == "User");
            var totalApplications = await _context.Applications.CountAsync();

            var dashboard = new AdminDashboardDto
            {
                TotalJobs = totalJobs,
                TotalApplicants = totalApplicants,
                TotalApplications = totalApplications
            };

            return Ok(dashboard);
        }

        // View all users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // View all job applications
        [HttpGet("applications")]
        public async Task<IActionResult> GetApplications()
        {
            var applications = await _context.Applications.ToListAsync();
            return Ok(applications);
        }
    }
}