using BooksProject.Shared;
using BooksWebApi.Abstractions;
using DataBaseModels;
using DataBaseManeger.Abstractions;

namespace BooksWebApi.Repositories
{
    public class FileBooksRepository(IDataBase<BookDetailsDto> dataBase) : IBooksRepository
    {        
        private IDataBase<BookDetailsDto> _dataBase = dataBase;

        public int AddBook(BookAddModelDto adedBook)
        {
            var book = new BookDetailsDto();
            _dataBase.InitializeId(book);

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

            _dataBase.AddItem(book);
            return book.Id;
        }

        public void AddBooks(List<BookDetailsDto> books)
        {
            _dataBase.AddItems(books);
        }
        public void DeleteAll()
        {
            _dataBase.DeleteAll();
        }

        public void DeleteBook(int id)
        {
            _dataBase.DeleteItem(id);
        }

        public GetBooksListModelDto<BookDetailsDto>? GetAll(GetListRequestDto getListRequest)
        {
            return _dataBase.GetAll(getListRequest);
        }

        public BookDetailsDto? GetById(int id)
        {
            return _dataBase.GetById(id);
        }

        public int GetLastItemId()
        {
            return _dataBase.GetLastId();
        }

    }
}
