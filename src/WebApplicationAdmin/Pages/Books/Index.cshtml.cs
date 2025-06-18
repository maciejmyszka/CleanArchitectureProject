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
    public class IndexModel : PageModel
    {
        private readonly CleanArchitectureProject.Infrastructure.Data.ApplicationDbContext _context;

        public IndexModel(CleanArchitectureProject.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Book> Book { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category).ToListAsync();
        }
    }
}
