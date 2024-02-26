using BooksProject.Shared;

namespace BooksWebApi.Abstractions
{
    public interface IBooksRepository
    {
        BookDetailsDto GetById(int id);
    }
}
