using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BookAPI.Repositories;
using Common.Models;
using System.Diagnostics.Contracts;

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
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var book = _bookRepository.GetRecordById(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(book);
        }


        // POST: api/book
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book is null.");
            }
            book.LastChangeAt = DateTime.UtcNow;
            _bookRepository.InsertRecord(book);
            return Ok(new { Message = "Book Created", Book = book });
        }
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            return Ok();
        }
    }
}
