using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Shared
{
    public class AddItemModelDto<Thing>
    {
        public Thing Item { get; set; } = default(Thing)!;

        public int Id { get; set; }
    }
}
