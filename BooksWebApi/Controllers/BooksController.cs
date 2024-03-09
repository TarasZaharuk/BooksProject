using BooksProject.Shared;
using BooksWebApi.Abstractions;
using BooksWebApi.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BooksWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;
        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet("books/{id}")]
        [ProducesResponseType<BookDetailsDto>(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            var book = _booksRepository.GetById(id);
            if (book == null)
                return NotFound();

            return Ok(book);     
        }


        [HttpGet("books")]
        [ProducesResponseType<GetBooksListModelDto>(StatusCodes.Status200OK)]
        public IActionResult GetAll(int skipBooks, int takeBooks, bool sortAscending, bool sortDescending, string? searchBook)
        {
            return Ok(_booksRepository.GetAll(skipBooks, takeBooks, sortAscending, sortDescending, searchBook));
        }


        [HttpPost("books")]
        [ProducesResponseType<BookAddModelDto>(StatusCodes.Status201Created)]
        public IActionResult AddBook(BookAddModelDto adedBook)
        {
            int id = _booksRepository.AddBook(adedBook);
            return Ok(id);
        }


        [HttpPost("books/generate/list")]
        [ProducesResponseType<BookAddModelDto>(StatusCodes.Status201Created)]
        public IActionResult GenerateBooksList(int generateBoooksCount)
        {
            int countOfBooks = BooksContainer.GenerateBooksList(generateBoooksCount);
            return Ok(countOfBooks);
        }


        [HttpDelete("books/{id}")]
        [ProducesResponseType<BookDetailsDto>(StatusCodes.Status204NoContent)]
        public IActionResult DeleteBook(int id)
        {
            _booksRepository.DeleteBook(id);
            return NoContent();
        }


        [HttpDelete("books")]
        [ProducesResponseType<BookDetailsDto>(StatusCodes.Status204NoContent)]
        public IActionResult DeleteAll()
        {
            _booksRepository.DeleteAll();
            return NoContent();
        }
    }
}
