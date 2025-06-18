using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CleanArchitectureProject.Domain.Entities;
using CleanArchitectureProject.Infrastructure.Data;

namespace WebApplicationAdmin.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly CleanArchitectureProject.Infrastructure.Data.ApplicationDbContext _context;

        public DetailsModel(CleanArchitectureProject.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);

            if (book is not null)
            {
                Book = book;

                return Page();
            }

            return NotFound();
        }
    }
}
