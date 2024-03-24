using BooksProject.Shared;

namespace DataBaseModels.Shared
{
    public class PageContentResponseModelDto
    {
        public List<BookDetailsDto> Books = [];

        public bool IsLastPage { get; set; }

        public int CountOfItemsBeforePage { get; set; }

    }
}

