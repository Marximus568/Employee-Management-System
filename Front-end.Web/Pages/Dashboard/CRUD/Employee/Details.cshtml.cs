using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Front_end.Web.Pages.Dashboard.CRUD.Employee
{
    public class DetailsModel : PageModel
    {
        private readonly Infrastructure.Persistence.Context.ApplicationDbContext _context;

        public DetailsModel(Infrastructure.Persistence.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public Domain.Entities.Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                Employee = employee;
            }
            return Page();
        }
    }
}
