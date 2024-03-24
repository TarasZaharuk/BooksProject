
namespace DataBaseModels.Shared
{
    public class PageStatusModelDto
    {
        public int Id { get; set; }

        public int LastId { get; set; }

        public int FirstId { get; set; }

        public int CountOfItems { get; set; }

        public readonly int _maxCountOfItems = 10000;

        public int AvailableSpace { get; set; }

        public PageStatusModelDto() 
        {
            AvailableSpace = _maxCountOfItems - CountOfItems;
            Id = 1;
        }
    }
}
