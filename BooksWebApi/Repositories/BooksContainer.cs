using BooksProject.Shared;

namespace BooksWebApi.Repositories
{
    public static class BooksContainer
    {
      public static List<BookDetailsDto> Books = new List<BookDetailsDto>();

        public static void InitializeId(BookDetailsDto book)
        {
            if (Books.Any()) 
            {
                book.Id = Books.Last().Id + 1;
            }
            else
            {
                book.Id = 1;
            }
        }
    }
}
