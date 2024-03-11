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

        public string? SearchBooks { get; set; }

        public int SkipBooks { get; set;}

        public int TakeBooks { get; set; }
    }
}
