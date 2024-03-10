using BooksProject.Shared;
using BooksWebApi.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BooksWebApi.Repositories
{
    public class InMemoryBooksRepository : IBooksRepository
    {

        public GetBooksListModelDto GetAll(GetListRequestDto getListRequest)
        {

            GetBooksListModelDto getBooksList = new GetBooksListModelDto();
            IEnumerable<BookDetailsDto> books = BooksContainer.Books;

            if (getListRequest.SearchBooks != null)
            {
                books = books.Where(book => book.Name.Contains(getListRequest.SearchBooks)); 
            }

            if (getListRequest.SortBooksCase == SortOrder.Ascending)
            {
                books = books.OrderBy(book => book.Name);
            }

            if (getListRequest.SortBooksCase == SortOrder.Descending)
            {
                books = books.OrderByDescending(book => book.Name);
            }

            getBooksList.Books = books.Skip(getListRequest.SkipBooks).Take(getListRequest.TakeBooks);
            getBooksList.TotalCount = books.Count();


            return getBooksList;
        }

        public BookDetailsDto? GetById(int id)
        {
            BookDetailsDto? searchedBook = BooksContainer.Books.SingleOrDefault(book => book.Id == id);
            return searchedBook;
        }

        public void DeleteBook(int id)
        {
            BookDetailsDto? book = BooksContainer.Books.SingleOrDefault(book => book.Id == id);
            if (book != null)
                BooksContainer.Books.Remove(book);
        }

        public void DeleteAll()
        {
            BooksContainer.Books.Clear();
        }

        public int AddBook(BookAddModelDto adedBook)
        {
            var book = new BookDetailsDto();
            BooksContainer.InitializeId(book);

            if (string.IsNullOrWhiteSpace(adedBook.Description))
            {
                adedBook.Description = $"{adedBook.Name}-{adedBook.Author}-{adedBook.DateOfPublishing.ToShortDateString()}";
            }
            book.Name = adedBook.Name;
            book.Description = adedBook.Description;
            book.Author = adedBook.Author;
            book.DateOfPublishing = DateOnly.FromDateTime(adedBook.DateOfPublishing);
            book.Rating = adedBook.Rating;
            book.Status = Status.Draft;

            BooksContainer.Books.Add(book);

            return book.Id;
        }

    }
}
