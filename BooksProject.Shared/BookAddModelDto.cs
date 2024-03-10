using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksProject.Shared
{
    public class BookAddModelDto
    {

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Author { get; set; }

        public DateTime DateOfPublishing { get; set; }

        public int Rating { get; set; }

    }
}
