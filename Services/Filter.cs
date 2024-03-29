using BooksProject.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Filter<T> where T : IGenericDBItemsType
    {
        public List<T> FilterItems(IEnumerable<T> items, GetListRequestDto getListRequest)
        {
            GetBooksListModelDto<T> getBooksList = new();
            getBooksList.Items = [];

            if (items.Any())
            {
                if (getListRequest.SearchItems != null)
                {
                    items = items.Where(book => book.Name.Contains(getListRequest.SearchItems));
                }

                if (getListRequest.NameSortOrder == SortOrder.Ascending)
                {
                    items = items.OrderBy(book => book.Name);
                }

                if (getListRequest.NameSortOrder == SortOrder.Descending)
                {
                    items = items.OrderByDescending(book => book.Name);
                }

            }
           
            return items.ToList();
        }
    }
}
