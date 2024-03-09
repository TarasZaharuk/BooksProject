using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksProject.Shared
{
    public class GetBooksListModelDto
    {
        public IEnumerable<BookDetailsDto> Books { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
