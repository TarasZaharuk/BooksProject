using BooksProject.Shared;

namespace DataBaseModels.Shared
{
    public class PageContentResponseModelDto<Thing>
    {
        public List<Thing> Items = [];

        public bool IsLastPage { get; set; }

        public int CountOfItemsBeforePage { get; set; }

    }
}

