using BooksProject.Shared;

namespace BooksWebApi.Abstractions
{
    public interface IBooksRepository
    {
        BookDetailsDto? GetById(int id);

        GetBooksListModelDto<BookDetailsDto>? GetAll(GetListRequestDto getListRequest);

        void DeleteBook(int id);

        void DeleteAll();

        int AddBook(BookAddModelDto book);

        void AddBooks(List<BookDetailsDto> books);

        int GetLastItemId();

    }
}
