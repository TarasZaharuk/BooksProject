namespace BookCore
{
    public class Book
    {
        private int _rating;
        private const int CountOfSymbolsPerPage = 1000;
        private readonly List<Page> _pages = new List<Page>();
        public Book(string name, string text, string author, DateOnly dateOfPublishing) : this(name, author, dateOfPublishing)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("Text is empty");
            Status = Status.Active;
            InnitPages(text);
        }
        public Book(string name, string author, DateOnly dateOfPublishing)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Name is empty");
            if (string.IsNullOrWhiteSpace(author))
                throw new Exception("Author is empty");
            if (dateOfPublishing.Year < 1991 || dateOfPublishing.Year > DateTime.Now.Year)
                throw new Exception($"Date of puplishing must be more than 1991 and less than {DateTime.Now.Year}");
            Status = Status.Draft;
            DateOfPublishing = dateOfPublishing;
            Author = author;
            Name = name;
        }
        public Status Status { get; private set; }
        public int Rating
        {
            get => _rating;
            set
            {
                if (value >= 1 && value <= 5)
                    _rating = value;
                else
                    throw new Exception("Rating can be from 1 to 5");
            }
        }
        public string Author { get; private set; }
        public DateOnly DateOfPublishing { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; } = null!;
        public string GetInfo() 
        {
            if (string.IsNullOrWhiteSpace(Description))
                return $"{Name}-{Author}-{DateOfPublishing.Year}";
            else return Description;
        }
        private void InnitPages(string text)
        {
            do
            {
                if (text.Length < CountOfSymbolsPerPage)
                {
                    _pages.Add(new Page { Text = text });
                    break;
                }
                int firstSpaceIndex = text.AsSpan().Slice(0, CountOfSymbolsPerPage).LastIndexOf(' ');
                _pages.Add(new Page { Text = new string(text.Take(firstSpaceIndex).ToArray()) });
                text = text.Remove(0, firstSpaceIndex);
            } while (text.Length != 0);
        }
        public int CountOfPages { get { return _pages.Count; } }

        public string GetText(int index)
        {
            if (index < _pages.Count)
            {
                return new string(_pages[index].Text);
            }
            return $"Index must be from {0} to {_pages.Count - 1} !";
        }
    }
}