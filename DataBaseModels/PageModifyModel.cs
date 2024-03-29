using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    public class PageModifyModel
    {
        public PageStatusModel PreviousPageStatus { get; set; } = null!;

        public PageStatusModel NewPageStatus { get; set; } = null!;

    }
}
