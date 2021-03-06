﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public SelectList Categories { get; set; }
        public SelectList Authors { get; set; }

        public void OnGet(int id)
        {
            Book = _context.Books.FirstOrDefault(b => b.Id == id);
            Categories = Book != null ? new SelectList(_context.Categories.ToList(), "Id", "Name", Book.CategoryId) : new SelectList(_context.Categories.ToList(), "Id", "Name");

            Authors = Book != null ? new SelectList(_context.Authors.ToList(), "Id", "Name", Book.AuthorId) : new SelectList(_context.Authors.ToList(), "Id", "Name");
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid) return Page();
                var oldBookInfo = await _context.Books.FindAsync(Book.Id);
                if (oldBookInfo == null) return NotFound();
                else 
                {
                    oldBookInfo.PublishedDate = Book.PublishedDate;
                    oldBookInfo.ISBN = Book.ISBN;
                    oldBookInfo.Name = Book.Name;
                    oldBookInfo.CategoryId = Book.CategoryId;
                    oldBookInfo.AuthorId = Book.AuthorId;
                }
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound();

            }
        }
    }
}