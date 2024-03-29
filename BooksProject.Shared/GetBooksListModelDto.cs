using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksProject.Shared
{
    public class GetBooksListModelDto<T>
    {
        public List<T> Items { get; set; } = null!;
        public int TotalCount { get; set; }

        public int LastId { get; set; }
    }
}
