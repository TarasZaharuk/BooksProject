using BooksProject.Shared;
using BooksWebApi.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BooksWebApi.Repositories
{
    public class SQLBooksRepository : IBooksRepository
    {
        private DBManipulator _dBManipulator;
        public SQLBooksRepository(DBManipulator dBManipulator) 
        {
            _dBManipulator = dBManipulator;
        }
        public int AddBook(BookAddModelDto adedBook)
        {
            var book = new BookDetailsDto();

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
            book.Id = 0;

            _dBManipulator.Add(book);
            _dBManipulator.SaveChanges();
            return book.Id;
        }

        public void AddBooks(List<BookDetailsDto> books)
        {
            _dBManipulator.AddRange(books);
            _dBManipulator.SaveChanges();
        }

        public void DeleteAll()
        {
            _dBManipulator.Books.ExecuteDelete();
            _dBManipulator.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            BookDetailsDto? book = _dBManipulator.Books.SingleOrDefault(book => book.Id == id);
            if (book != null)
            {
                _dBManipulator.Remove(book);
                _dBManipulator.SaveChanges();
            }           
        }

        public GetBooksListModelDto<BookDetailsDto>? GetAll(GetListRequestDto getListRequest)
        {
            GetBooksListModelDto<BookDetailsDto> getBooksListModel = new();

            List<BookDetailsDto> books = _dBManipulator.Books.Skip(getListRequest.SkipItems).Take(getListRequest.TakeItems).ToList();
            if (!string.IsNullOrWhiteSpace(getListRequest.SearchItems))
            {
                books = _dBManipulator.Books.Where(book => book.Name.Contains(getListRequest.SearchItems)).Skip(getListRequest.SkipItems).Take(getListRequest.TakeItems).ToList();
            }
            if (getListRequest.NameSortOrder == SortOrder.Ascending)
            {
                books = books.OrderBy(book => book.Name).ToList();
            }
            if (getListRequest.NameSortOrder == SortOrder.Descending)
            {
                books = books.OrderByDescending(book => book.Name).ToList();
            }

            getBooksListModel.TotalCount = _dBManipulator.Books.Count();
            getBooksListModel.Items = books;
            return getBooksListModel;
        }

        public BookDetailsDto? GetById(int id)
        {
           return  _dBManipulator.Books.SingleOrDefault(book => book.Id == id);
        }

        public int GetLastItemId()
        {
            return 0;
        }
    }
}
