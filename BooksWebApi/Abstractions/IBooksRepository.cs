using BooksProject.Shared;

namespace BooksWebApi.Abstractions
{
    public interface IBooksRepository
    {
        BookDetailsDto? GetById(int id);
        List<BookDetailsDto> GetAll();

        void DeleteBook(int id);

        void DeleteAll();

        int AddBook(BookAddModelDto book);
    }
}
