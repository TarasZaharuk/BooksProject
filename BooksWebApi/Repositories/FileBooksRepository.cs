using BooksProject.Shared;
using BooksWebApi.Abstractions;
using DataBaseModels.Shared;
using DBMeneger;

namespace BooksWebApi.Repositories
{
    public class FileBooksRepository : IBooksRepository
    {
        private DataBaseManipulator _dbManipulator = new();

        public int AddBook(BookAddModelDto adedBook)
        {
            var book = new BookDetailsDto();
            _dbManipulator.InitializeId(book);

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

            AddItemModelDto<BookDetailsDto> addItem = new() {Item = book,Id = book.Id };
            PageBuilder.AddItem(addItem);
            return book.Id;
        }

        public void DeleteAll()
        {
            _dbManipulator.DeleteAll();
        }

        public void DeleteBook(int id)
        {
            _dbManipulator.DeleteBook(id);
        }

        public GetBooksListModelDto? GetAll(GetListRequestDto getListRequest)
        {
            return _dbManipulator.GetAll(getListRequest);
        }

        public BookDetailsDto? GetById(int id)
        {
            return _dbManipulator.GetById(id);
        }

    }
}
