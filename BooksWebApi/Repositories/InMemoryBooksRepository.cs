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

        public GetBooksListModelDto<BookDetailsDto> GetAll(GetListRequestDto getListRequest)
        {

            GetBooksListModelDto<BookDetailsDto> getBooksList = new GetBooksListModelDto<BookDetailsDto>();
            IEnumerable<BookDetailsDto> books = BooksContainer.Books;

            if (getListRequest.SearchItems != null)
            {
                books = books.Where(book => book.Name.Contains(getListRequest.SearchItems)); 
            }

            if (getListRequest.NameSortOrder == SortOrder.Ascending)
            {
                books = books.OrderBy(book => book.Name);
            }

            if (getListRequest.NameSortOrder == SortOrder.Descending)
            {
                books = books.OrderByDescending(book => book.Name);
            }

            getBooksList.Items = books.Skip(getListRequest.SkipItems).Take(getListRequest.TakeItems).ToList();
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

        public void AddBooks(List<BookDetailsDto> books)
        {
            BooksContainer.Books.AddRange(books);
        }

        public int GetLastItemId()
        {
            if(BooksContainer.Books.Count > 0)
                return BooksContainer.Books.Last().Id;
            else return 0;
        }
    }
}
