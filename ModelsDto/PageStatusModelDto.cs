
namespace DataBaseModels.Shared
{
    public class PageStatusModelDto
    {
        public int Id { get; set; }

        public int LastId { get; set; }

        public int FirstId { get; set; }

        public int CountOfBooks { get; set; }

        public readonly int _maxCountOfBooks = 10000;

        public int AvailableSpace { get; set; }

        public PageStatusModelDto() 
        {
            AvailableSpace = _maxCountOfBooks - CountOfBooks;
            Id = 1;
        }
    }
}
