using BooksProject.Shared;
using DataBaseModels.Shared;
using System.Text.Json;


namespace DBMeneger
{
    public static class PageManeger
    {
        private static string DataBasePagesBasePath = null!;
        public static void SetDataPagesBasePath(string path)
        {
            DataBasePagesBasePath = path;
        }
        public static List<Thing> GetAllPagesContent<Thing>()
        {
            DataBaseStatusModelDto dataState = MetaDataManeger.GetDataBaseMetaData();
            List<Thing> items = [];
            foreach (var page in dataState.Pages)
            {
                items.AddRange(GetPageContent<Thing>(page));
            }
            return items;
        }

        public static PageStatusModelDto GetLastNotEmptyPage()
        {
            DataBaseStatusModelDto dataState = MetaDataManeger.GetDataBaseMetaData();
            PageStatusModelDto? page = dataState.Pages.AsEnumerable().Reverse().FirstOrDefault(page => page.CountOfItems > 0);

            return page ?? throw new Exception("page not found");
        }

        public static List<Thing> GetPageContent<Thing>(PageStatusModelDto page)
        {
            if (!File.Exists(DataBasePagesBasePath + page.Id))
                return new List<Thing>();

            string jsonString;
            using (StreamReader streamReader = new(DataBasePagesBasePath + page.Id))
            {
                jsonString = streamReader.ReadToEnd();
            }
            List<Thing> items = [];

            if (!string.IsNullOrWhiteSpace(jsonString))
                items = JsonSerializer.Deserialize<List<Thing>>(jsonString) ?? throw new Exception("items is null");

            return items;
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

        public static List<Thing> GetLastPageContent<Thing>()
        {
            List<Thing> items = GetPageContent<Thing>(GetLastPage());
            return items;
        }

        public static PageContentResponseModelDto<Thing> GetPageContentInDiapazon<Thing>(PageContentRequestModelDto pageContentRequest)
        {
            PageContentResponseModelDto<Thing> pageContentResponse = new();
            PageContentDiapazonResponseModelDto pageDiapazon = GetPageInDiapazon(pageContentRequest.SkipItems, pageContentRequest.TakeItems);
            PageStatusModelDto page = pageDiapazon.Page;
            if (pageContentRequest.RequestPageContentMode == RequestPageContentMode.NextPage)
            {
                page = GetNextPage(page) ?? throw new Exception("page is null");
            }
            List<Thing> items = GetPageContent<Thing>(page);
            pageContentResponse.Items = items;
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
                countOfCheckedItems += page.CountOfItems;
                if (countOfCheckedItems >= takeItems + skipItems)
                {
                    pageState = page;
                    if (pageState.CountOfItems < takeItems && page.Id != MetaDataManeger.GetDataBaseMetaData().Pages.Last().Id)
                        throw new Exception("Not Available DataPage");
                    break;
                }
            }
            pageDiapazon.Page = pageState ?? GetLastNotEmptyPage();
            pageDiapazon.CountOfItemsBeforePage = countOfCheckedItems - pageDiapazon.Page.CountOfItems;
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
