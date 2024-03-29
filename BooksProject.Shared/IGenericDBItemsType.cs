using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksProject.Shared
{
    public interface IGenericDBItemsType
    {
        string Name { get; set; }

        int Id { get; set; }
    }
}
