using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    public class DataBaseStatusModel
    {
        public int FirstId { get; set; }

        public int LastId { get; set; }

        public PageStatusModel AvailablePage { get; set; } = null!;

        public List<PageStatusModel> Pages { get; set; } = null!;
    }
}
