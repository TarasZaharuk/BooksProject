using BooksProject.Shared;
using DataBaseModels.Shared;
using System.Text.Json;


namespace DBMeneger
{
    public class PageManeger
    {
        private static string DataBasePagesBasePath = null!;
        public static void SetDataPagesBasePath(string path)
        {
            DataBasePagesBasePath = path;
        }
        public static List<BookDetailsDto> GetAllPagesContent()
        {
            DataBaseStatusModelDto dataState = MetaDataManeger.GetDataBaseMetaData();
            List<BookDetailsDto> books = [];
            foreach (var page in dataState.Pages)
            {
                books.AddRange(GetPageContent(page));
            }
            return books;
        }

        public static PageStatusModelDto GetLastNotEmptyPage()
        {
            DataBaseStatusModelDto dataState = MetaDataManeger.GetDataBaseMetaData();
            PageStatusModelDto? page = dataState.Pages.AsEnumerable().Reverse().FirstOrDefault(page => page.CountOfBooks > 0);

            return page ?? throw new Exception("page not found");
        }

        public static List<BookDetailsDto> GetPageContent(PageStatusModelDto page)
        {
            if (!File.Exists(DataBasePagesBasePath + page.Id))
                return new List<BookDetailsDto>();

            string jsonString;
            using (StreamReader streamReader = new(DataBasePagesBasePath + page.Id))
            {
                jsonString = streamReader.ReadToEnd();
            }
            List<BookDetailsDto> books = [];

            if (!string.IsNullOrWhiteSpace(jsonString))
                books = JsonSerializer.Deserialize<List<BookDetailsDto>>(jsonString) ?? throw new Exception("books is null");

            return books;
        }

        public static PageStatusModelDto GetLastPage()
        {
            DataBaseStatusModelDto dataState = MetaDataManeger.GetDataBaseMetaData();
            return dataState.Pages.Last();
        }

        public static int GetLastPageAvailableSpace()
        {
            return GetLastPage().AvailableSpace;
        }
        public static int GetLastPageId()
        {
            return GetLastPage().Id;
        }

        public static List<BookDetailsDto> GetLastPageContent()
        {
            return GetPageContent(GetLastPage());
        }

        public static PageContentResponseModelDto GetPageContentInDiapazon(PageContentRequestModelDto pageContentRequest)
        {
            PageContentResponseModelDto pageContentResponse = new();
            PageContentDiapazonResponseModelDto pageDiapazon = GetPageInDiapazon(pageContentRequest.SkipItems, pageContentRequest.TakeItems);
            PageStatusModelDto page = pageDiapazon.Page;
            if (pageContentRequest.RequestPageContentMode == RequestPageContentMode.NextPage)
            {
                page = GetNextPage(page) ?? throw new Exception("page is null");
            }
            List<BookDetailsDto> books = GetPageContent(page);
            pageContentResponse.Books = books;
            if (page.Id == GetLastNotEmptyPage().Id)
            {
                pageContentResponse.IsLastPage = true;
            }
            pageContentResponse.CountOfItemsBeforePage = pageDiapazon.CountOfItemsBeforePage;
            return pageContentResponse;
        }

        private static PageContentDiapazonResponseModelDto GetPageInDiapazon(int skipItems, int takeItems)
        {
            int countOfCheckedItems = 0;
            PageContentDiapazonResponseModelDto pageDiapazon = new();
            PageStatusModelDto pageState = null!;
            List<PageStatusModelDto> pages = MetaDataManeger.GetDataBaseMetaData().Pages;

            foreach (var page in pages)
            {
                countOfCheckedItems += page.CountOfBooks;
                if (countOfCheckedItems >= takeItems + skipItems)
                {
                    pageState = page;
                    if (pageState.CountOfBooks < takeItems && page.Id != MetaDataManeger.GetDataBaseMetaData().Pages.Last().Id)
                        throw new Exception("Not Available DataPage");
                    break;
                }
            }
            pageDiapazon.Page = pageState ?? GetLastNotEmptyPage();
            pageDiapazon.CountOfItemsBeforePage = countOfCheckedItems - pageDiapazon.Page.CountOfBooks;
            return pageDiapazon;
        }

        private static PageStatusModelDto? GetNextPage(PageStatusModelDto curentPage)
        {
            DataBaseStatusModelDto dataBaseStatus = MetaDataManeger.GetDataBaseMetaData();
            PageStatusModelDto? nextPage = dataBaseStatus.Pages.SingleOrDefault(page => page.Id == curentPage.Id + 1);
            if (nextPage == null)
                throw new Exception();
            return nextPage;
        }

    }
}
