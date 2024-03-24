using System;
using BooksProject.Shared;

namespace DataBaseModels.Shared
{
    public class DataBaseStatusModelDto
    {
        public int FirstId { get; set; }

        public int LastId { get; set; }

        public int AvailablePageId { get; set; }

        public List<PageStatusModelDto> Pages { get; set; } = [];
    }
}


