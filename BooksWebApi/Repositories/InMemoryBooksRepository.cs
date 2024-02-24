using BooksProject.Shared;
using BooksWebApi.Abstractions;

namespace BooksWebApi.Repositories
{
    public class InMemoryBooksRepository : IBooksRepository
    {
        public BookDetailsDto GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
