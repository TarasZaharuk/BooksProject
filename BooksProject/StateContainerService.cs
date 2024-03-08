using BookCore;

namespace BooksProject
{
        public class StateContainerService
        {
        public Book Book { get; set; } = null!;

            public event Action OnStateChange = null!;

            public void SetValue(Book book)
            {
                Book = book;
                NotifyStateChanged();
            }

            private void NotifyStateChanged() => OnStateChange?.Invoke();
        }
    }
