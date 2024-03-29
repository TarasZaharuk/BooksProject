using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BooksProject.Shared;
using DataBaseManeger.Abstractions;
using DataBaseModels;
using Services;

namespace DataBaseManeger
{
    internal class PageManeger<T> : IPageManeger<T> where T : IGenericDBItemsType
    {
        private readonly Filter<T> _filter = new();
        public static string DataBasePagesBasePath { get; set; } = null!;

        public void SetPath(string dataBasePagesPath)
        {
            DataBasePagesBasePath = dataBasePagesPath;
        }

        public PageStatusModel AddItem(T newItem, DataBaseStatusModel dataBaseStatus)
        {
            List<T> items = new List<T> { newItem };
            List<T> newItems = items;

            return AddItems(newItems, dataBaseStatus).PageStatus;
        }

        public AddItemsModel<T> AddItems(List<T> newItems, DataBaseStatusModel dataBaseStatus)
        {
            PageStatusModel page = new();
            AddItemsModel<T> addedItems = new();
            List<T> items = [];

            if (GetLastNotEmptyPage(dataBaseStatus).AvailableSpace > 0)
                items = GetLastNotEmptyPageContent(dataBaseStatus);

            int previousCount = items.Count;
            items.AddRange(newItems.Take(GetLastPageAvailableSpace(dataBaseStatus)));
            page.CountOfItems = items.Count;
            page.LastId = items.Last().Id;
            page.FirstId = items.First().Id;
            page.Id = dataBaseStatus.Pages.Last().Id;

            WriteItemsInFile(dataBaseStatus, items);
            addedItems.PageStatus = page;
            addedItems.CountOfAddedItems = items.Count - previousCount;
            return addedItems;
        }

        public void ClearAllPages()
        {
            DirectoryInfo dir = new DirectoryInfo(DataBasePagesBasePath);
            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }
        }
        
        public PageModifyModel DeleteItem(int id, DataBaseStatusModel dataBaseStatus)
        {     
            //find dataPage where is item can be
            PageStatusModel? dataPage = dataBaseStatus.Pages.SingleOrDefault(page => page.FirstId <= id && page.LastId >= id);
            if (dataPage == null)
                throw new Exception("dataPage is null");
            PageModifyModel modifiedPage = new();
            PageStatusModel modifiedPageStatus = dataPage;
            modifiedPage.PreviousPageStatus = dataPage;


            
            List<T> updatedtems = GetPageContent(dataPage);
            T? item = updatedtems.SingleOrDefault(item => item.Id == id);
            if (item == null)
                throw new Exception("dataPage is null");

            updatedtems.Remove(item);
            modifiedPageStatus.CountOfItems -= 1;
            modifiedPage.NewPageStatus = modifiedPageStatus;

            UpdatePage(modifiedPageStatus, updatedtems, dataBaseStatus);
            return modifiedPage;
        }

        public List<T> GetAllPagesContent(DataBaseStatusModel dataBaseStatus)
        {
            List<T> items = [];
            foreach (var page in dataBaseStatus.Pages)
            {
                items.AddRange(GetPageContent(page));
            }
            return items;
        }

        public List<T> GetPageContentInDiapazon(GetListRequestDto filter, DataBaseStatusModel dataBaseStatus)
        {
            PageContentDiapazonResponseModel pageDiapazon = GetPageInDiapazon(filter.SkipItems, filter.TakeItems, dataBaseStatus);

            List<T> items = [];
            int skip = filter.SkipItems - pageDiapazon.CountOfItemsBeforePage;
            PageStatusModel page = pageDiapazon.Page;
            items.AddRange(GetPageContent(page));
            items = _filter.FilterItems(items, filter);
            items =  items.Skip(skip).Take(filter.TakeItems).ToList();
            if (items.Count == filter.TakeItems)
                return items;

            int take = filter.TakeItems - items.Count;
            PageStatusModel? curentDataPage = page;
            do
            {
                curentDataPage = GetNextPage(curentDataPage, dataBaseStatus);
                if (curentDataPage == null)
                    break;

                items.AddRange(_filter.FilterItems(GetPageContent(curentDataPage),filter).Take(take));
                take = filter.TakeItems - items.Count;
            } while (items.Count != filter.TakeItems || curentDataPage.Id == GetLastNotEmptyPage(dataBaseStatus).Id);
            
            return items;
        }


        public T? GetById(int id, DataBaseStatusModel dataBaseStatus)
        {
            PageStatusModel? page = dataBaseStatus.Pages.SingleOrDefault(page => page.FirstId <= id && page.LastId >= id);
            if (page == null)
                throw new Exception("dataPage is null");
            List<T> items = GetPageContent(page);
            T? item = items.SingleOrDefault(thing => thing.Id == id);

            return item;
        }




        private PageContentDiapazonResponseModel GetPageInDiapazon(int skipItems, int takeItems, DataBaseStatusModel dataBaseStatus)
        {
            int countOfCheckedItems = 0;
            PageContentDiapazonResponseModel pageDiapazon = new();
            PageStatusModel pageState = null!;
            List<PageStatusModel> pages = dataBaseStatus.Pages;

            foreach (var page in pages)
            {
                countOfCheckedItems += page.CountOfItems;
                if (countOfCheckedItems >= takeItems + skipItems)
                {
                    pageState = page;
                    if (pageState.CountOfItems < takeItems && page.Id != dataBaseStatus.Pages.Last().Id)
                        throw new Exception("Not Available DataPage");
                    break;
                }
            }
            pageDiapazon.Page = pageState ?? GetLastNotEmptyPage(dataBaseStatus);
            pageDiapazon.CountOfItemsBeforePage = countOfCheckedItems - pageDiapazon.Page.CountOfItems;
            return pageDiapazon;
        }

        private List<T> GetPageContent(PageStatusModel page)
        {
            if (!File.Exists(DataBasePagesBasePath + page.Id))
                return [];

            string jsonString;
            using (StreamReader streamReader = new(DataBasePagesBasePath + page.Id))
            {
                jsonString = streamReader.ReadToEnd();
            }
            List<T> items = [];

            if (!string.IsNullOrWhiteSpace(jsonString))
                items = JsonSerializer.Deserialize<List<T>>(jsonString) ?? throw new Exception("updatedtems is null");

            return items;
        }

        private void UpdatePage(PageStatusModel page, List<T> addItems, DataBaseStatusModel dataBaseStatus)
        {
            using (StreamWriter streamWriter = File.CreateText(GetPagePath(dataBaseStatus,page)))
            {
                streamWriter.Write(JsonSerializer.Serialize(addItems));
            }
        }

        private PageStatusModel GetLastNotEmptyPage(DataBaseStatusModel dataBaseStatus)
        {
            PageStatusModel? page = dataBaseStatus.Pages.AsEnumerable().Reverse().FirstOrDefault(page => page.CountOfItems > 0);

            return page ?? new PageStatusModel();
        }

        private List<T> GetLastNotEmptyPageContent(DataBaseStatusModel dataBaseStatus)
        {
            PageStatusModel page = GetLastNotEmptyPage(dataBaseStatus);
            return GetPageContent(page);
        }

        private void WriteItemsInFile(DataBaseStatusModel dataBaseStatus, List<T> addedItems)
        {
            using (StreamWriter streamWriter = File.CreateText(GetPagePath(dataBaseStatus)))
            {
                streamWriter.Write(JsonSerializer.Serialize(addedItems));
            }
        }

        
        private PageStatusModel? GetNextPage(PageStatusModel curentPage, DataBaseStatusModel dataBaseStatus)
        {
            PageStatusModel? nextPage = dataBaseStatus.Pages.SingleOrDefault(page => page.Id == curentPage.Id + 1);

            return nextPage;
        }
        
        private string GetPagePath(DataBaseStatusModel dataBaseStatus)
        {
            PageStatusModel? page = dataBaseStatus.Pages.SingleOrDefault(page => page.Id == dataBaseStatus.AvailablePage.Id);
            if (page == null)
                throw new Exception("dataPage is null");
            return DataBasePagesBasePath + page.Id;
        }

        private string GetPagePath(DataBaseStatusModel dataBaseStatus, PageStatusModel dataPage)
        {
            PageStatusModel? page = dataBaseStatus.Pages.SingleOrDefault(page => page.Id == dataPage.Id);
            if (page == null)
                throw new Exception("dataPage is null");
            return DataBasePagesBasePath + page.Id;
        }

        private int GetLastPageAvailableSpace(DataBaseStatusModel dataBaseStatus)
        {
            return dataBaseStatus.Pages.Last().AvailableSpace;
        }

        private List<T> GetLastPageContent(DataBaseStatusModel dataBaseStatus)
        {
            return GetPageContent(dataBaseStatus.Pages.Last());
        }


    }
}
