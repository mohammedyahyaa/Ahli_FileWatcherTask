using FileTaskApi.Data;
using FileTaskApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileTaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileBankTaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FileBankTaskController(ApplicationDbContext context)
        {
            _context = context;

        }



        // GET all Employees Data 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            var AllEmployees = await _context.Employees.ToListAsync(); 
            return Ok(AllEmployees);
        }


        // Get All Employees whom have .net developer title 
        [HttpGet("{title}")]
        public async Task<ActionResult<Employee>> GetEmployeeByTitle(string title)
        {

          var Employee =  _context.Employees.Where(t => t.title.Contains(title)).ToList();

            

            if (Employee == null)
            {
                return NotFound();
            }

            return Ok(Employee);
        }

    }
}
