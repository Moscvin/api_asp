using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BookAPI.Repositories;
using Common.Models;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMongoRepository<Book> _bookRepository;

        public BookController(IMongoRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/book
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _bookRepository.GetAllRecords();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/book
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book is null.");
            }

            _bookRepository.InsertRecord(book);
            return Ok(new { Message = "Book Created", Book = book });
        }
    }
}
