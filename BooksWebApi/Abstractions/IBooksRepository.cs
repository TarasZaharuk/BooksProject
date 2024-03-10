using BooksProject.Shared;

namespace BooksWebApi.Abstractions
{
    public interface IBooksRepository
    {
        BookDetailsDto? GetById(int id);

        GetBooksListModelDto GetAll(GetListRequestDto getListRequest);

        void DeleteBook(int id);

        void DeleteAll();

        int AddBook(BookAddModelDto book);

    }
}
