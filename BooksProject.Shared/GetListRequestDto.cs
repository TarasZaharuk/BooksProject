using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksProject.Shared
{
    public class GetListRequestDto
    {
        public SortOrder NameSortOrder { get; set; }

        public string? SearchItems { get; set; }

        public int SkipItems { get; set;}

        public int TakeItems { get; set; }
    }
}
