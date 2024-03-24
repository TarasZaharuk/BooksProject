using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Shared
{
    public class PageContentDiapazonResponseModelDto
    {
        public PageStatusModelDto Page { get; set; } = new();

        public int CountOfItemsBeforePage { get; set; }
    }
}
