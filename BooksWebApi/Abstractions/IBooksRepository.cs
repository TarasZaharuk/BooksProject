using BooksProject.Shared;

namespace BooksWebApi.Abstractions
{
    public interface IBooksRepository
    {
        BookDetailsDto? GetById(int id);

        GetBooksListModelDto? GetAll(int skipBooks, int takeBooks, bool UpperCaseSort, bool LowerCaseSort, string? searchBook);

        void DeleteBook(int id);

        void DeleteAll();

        int AddBook(BookAddModelDto book);

    }
}
