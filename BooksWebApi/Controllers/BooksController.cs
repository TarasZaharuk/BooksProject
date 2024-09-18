using BooksProject.Shared;
using BooksWebApi.Abstractions;
using Services;
using Microsoft.AspNetCore.Mvc;


namespace BooksWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class BooksController(IBooksRepository booksRepository) : ControllerBase
    {
        private readonly IBooksRepository _booksRepository = booksRepository;

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
        [ProducesResponseType<GetBooksListModelDto<BookDetailsDto>>(StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery]GetListRequestDto getListRequest)
        {
            return Ok(_booksRepository.GetAll(getListRequest));
        }


        [HttpPost("books")]
        [ProducesResponseType<BookAddModelDto>(StatusCodes.Status201Created)]
        public IActionResult AddBook(BookAddModelDto adedBook)
        {
            int id = _booksRepository.AddBook(adedBook);
            return Ok(id);
        }


        [HttpPost("books/generate")]
        [ProducesResponseType<BookAddModelDto>(StatusCodes.Status201Created)]
        public IActionResult GenerateBooksList(int generateBooksCount)
        {
            int count = generateBooksCount;
            BooksGenerator booksGenerator = new BooksGenerator();
            List<BookDetailsDto> books = [];
            do
            {
                books = booksGenerator.GenerateBooksList(_booksRepository.GetLastItemId(), count,false);
                _booksRepository.AddBooks(books);
                count -= books.Count();
            } while (count != 0);
            
            return Ok(generateBooksCount);
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
