using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    public class AddItemsModel<T>
    {
        public int CountOfAddedItems { get; set; }

        public PageStatusModel PageStatus { get; set; } = null!;
    }
}
