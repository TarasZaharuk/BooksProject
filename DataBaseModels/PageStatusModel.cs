using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    public class PageStatusModel
    {
        public int Id { get; set; } = 1;

        public int FirstId { get; set; }

        public int LastId { get; set; }

        public readonly int _maxItemsCount = 10000;

        public int CountOfItems { get; set; }

        public int AvailableSpace { get { return _maxItemsCount - CountOfItems; } private set { } }
    }
}
