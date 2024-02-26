using BooksProject.Shared;
using BooksWebApi.Abstractions;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("/books/{id}")]
        [ProducesResponseType<BookDetailsDto>(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            //_booksRepository.GetById(id);
            var book = new BookDetailsDto
            {
                Name = "Thinking in systems",
                Description = "description"
            };

            return Ok(book);
        }
    }
}
