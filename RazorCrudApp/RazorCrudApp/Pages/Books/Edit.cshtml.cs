﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorCrudApp.AppDb;
using RazorCrudApp.Models;

namespace RazorCrudApp.Pages.Books
{
    public class EditModel : PageModel
    {
        private AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        public void OnGet(int id)
        {
            Book = _context.Books.FirstOrDefault(b => b.Id == id);
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid) return Page();
                var oldBookInfo = await _context.Books.FindAsync(Book.Id);
                if (oldBookInfo == null) return BadRequest();
                else 
                {
                    oldBookInfo.PublishedDate = Book.PublishedDate;
                    oldBookInfo.ISBN = Book.ISBN;
                    oldBookInfo.Name = Book.Name;
                }
                var isUpdated = await _context.SaveChangesAsync();
                if (isUpdated > 0) return RedirectToPage("Index");
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest();

            }
        }
    }
}