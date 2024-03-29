using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    public class PageContentDiapazonResponseModel
    {
        public PageStatusModel Page { get; set; } = null!;

        public int CountOfItemsBeforePage { get; set; }
    }
}
