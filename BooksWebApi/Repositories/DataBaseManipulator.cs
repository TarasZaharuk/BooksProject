using BooksProject.Shared;
using DBMeneger;
using DataBaseModels.Shared;

namespace BooksWebApi.Repositories
{
    public class DataBaseManipulator
    {
        private DataBaseStatusModelDto? _dataBaseStatus;
        private List<BookDetailsDto>? Books = [];
        private static GetBooksListModelDto _booksResponse = new();

        public BookDetailsDto InitializeId(BookDetailsDto book)
        {
            book.Id = MetaDataManeger.GetLastId() + 1;
            return book;
        }

        public int GenerateBooksList(int generateBoooksCount)
        {
            List<BookDetailsDto>? books = [];
            int availableSpace = PageManeger.GetLastPageAvailableSpace();
            int countOfBooks = 0;
            do
            {
                if (generateBoooksCount < availableSpace)
                    books = BooksGenerator.GenerateBooksList(generateBoooksCount);
                else
                    books = BooksGenerator.GenerateBooksList(PageManeger.GetLastPageAvailableSpace());

                if (books == null)
                    throw new Exception("books is null");

                PageBuilder.AddBooks(books);

                generateBoooksCount -= books.Count;
                countOfBooks += books.Count;
            } while (generateBoooksCount != 0);
            return countOfBooks;
        }
        public List<BookDetailsDto> GetAll()
        {
            return PageManeger.GetAllPagesContent();
        }

        public GetBooksListModelDto GetAll(GetListRequestDto getListRequest)
        {
            GetBooksListModelDto getBooksList = new();

            if (MetaDataManeger.GetTotalCount() == 0)
                return getBooksList;

            List<BookDetailsDto> books = [];
            
            do
            {

                PageContentRequestModelDto pageContentRequest = new();
                PageContentResponseModelDto pageContentResponse = new();
                pageContentRequest.TakeItems = getListRequest.TakeBooks;
                pageContentRequest.SkipItems = getListRequest.SkipBooks;
                if (!books.Any())
                {
                    pageContentResponse = PageManeger.GetPageContentInDiapazon(pageContentRequest);
                    books = pageContentResponse.Books;
                    getListRequest.SkipBooks -= pageContentResponse.CountOfItemsBeforePage;
                    books = FilterItems(books, getListRequest);
                }
                else
                {
                    pageContentRequest.RequestPageContentMode = RequestPageContentMode.NextPage;
                    pageContentResponse = PageManeger.GetPageContentInDiapazon(pageContentRequest);
                    getListRequest.SkipBooks -= pageContentResponse.CountOfItemsBeforePage;
                    books.AddRange(FilterItems(pageContentResponse.Books, getListRequest));
                }

                if (pageContentResponse.IsLastPage)
                    break;

            } while (books.Count != getListRequest.TakeBooks);


            getBooksList.Books = books;
            getBooksList.TotalCount = MetaDataManeger.GetTotalCount();



            return getBooksList;
        }

        private List<BookDetailsDto> FilterItems(IEnumerable<BookDetailsDto> books, GetListRequestDto getListRequest)
        {
            GetBooksListModelDto getBooksList = new();
            getBooksList.Books = [];

            if (books.Any())
            {
                if (getListRequest.SearchBooks != null)
                {
                    books = books.Where(book => book.Name.Contains(getListRequest.SearchBooks));
                }

                if (getListRequest.NameSortOrder == SortOrder.Ascending)
                {
                    books = books.OrderBy(book => book.Name);
                }

                if (getListRequest.NameSortOrder == SortOrder.Descending)
                {
                    books = books.OrderByDescending(book => book.Name);
                }

                books = books.Skip(getListRequest.SkipBooks).Take(getListRequest.TakeBooks);

            }

            return books.ToList();
        }

        public BookDetailsDto? GetById(int id)
        {
            _dataBaseStatus = MetaDataManeger.GetDataBaseMetaData();
            _dataBaseStatus ??= new DataBaseStatusModelDto();
            PageStatusModelDto? page = _dataBaseStatus.Pages.SingleOrDefault(page => page.LastId >= id && page.FirstId <= id);
            List<BookDetailsDto>? books = [];
            if (page != null)
                books = PageManeger.GetPageContent(page);

            if (books != null)
            {
                BookDetailsDto? book = books.SingleOrDefault(book => book.Id == id);

                return book ?? throw new Exception("Book not found");
            }

            return null;
        }

        public void DeleteAll()
        {
            PageBuilder.ClearAllPages();
        }

        public void DeleteBook(int id)
        {
            _dataBaseStatus = MetaDataManeger.GetDataBaseMetaData();
            PageStatusModelDto? page = _dataBaseStatus.Pages.SingleOrDefault(page => page.LastId >= id && page.FirstId <= id);

            if (page == null)
                throw new Exception("page not founded");

            List<BookDetailsDto>? books = PageManeger.GetPageContent(page);

            BookDetailsDto? book = books.SingleOrDefault(book => book.Id == id);

            if (book == null)
                throw new Exception("Books not found");

            books.Remove(book);
            PageBuilder.UpdatePage(books, page);
        }
    }
}
