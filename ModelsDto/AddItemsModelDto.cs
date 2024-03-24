using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Shared
{
    public class AddItemsModelDto<Thing>
    {
        public List<Thing> Items { get; set; } = [];

        public int LastId { get; set; }

        public int FirstId { get; set; }
    }
}
