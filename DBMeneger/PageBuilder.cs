using BooksProject.Shared;
using DataBaseModels.Shared;
using System.Text.Json;

namespace DBMeneger
{
    public static class PageBuilder
    {
        private static string DataPagesBasePath { get; set; } = null!;

        public static void SetDataPagesBasePath(string path)
        {
            DataPagesBasePath = path;
        }
        public static void AddItems<Thing>(AddItemsModelDto<Thing> addItems)
        {
            AddItemsModelDto<Thing> addItemsModelDto = new();
            do
            {
                if (PageManeger.GetLastPage().AvailableSpace == 0)
                    throw new Exception("AvailableSpace can not be null");

                List<Thing> items = PageManeger.GetLastPageContent<Thing>();
                items.AddRange(addItems.Items.Take(PageManeger.GetLastPageAvailableSpace()));
                addItems.Items = addItems.Items.Skip(PageManeger.GetLastPageAvailableSpace()).ToList();
                addItemsModelDto.Items = items;
                addItemsModelDto.FirstId = items.First().GetId();
                addItemsModelDto.LastId  = items.Last().GetId();
                WriteItemsInFile(DataPagesBasePath + PageManeger.GetLastPageId(), addItemsModelDto);
            } while (addItems.Items.Count != 0);

        }

        public static void AddItem<Thing>(AddItemModelDto<Thing> addItem)
        {
            List<Thing> items = new List<Thing>{addItem.Item};
            AddItemsModelDto<Thing> addItems = new() { Items = items,LastId = addItem.Id,FirstId = addItem.Id};
            AddItems(addItems);
        }

        private static void WriteItemsInFile<Thing>(string path, AddItemsModelDto<Thing> addItems)
        {
            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(JsonSerializer.Serialize(addItems.Items));
            }

            MetaDataManeger.MetaDataHasChanged(addItems.LastId, addItems.Items.Count, addItems.FirstId);
            if (PageManeger.GetLastPageAvailableSpace() == 0)
                CreatePage();
        }

        private static void AddPageInFile(string path)
        {
            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(string.Empty);
            }
        }

        private static int CreatePage()
        {
            DataBaseStatusModelDto dataState = MetaDataManeger.GetDataBaseMetaData();
            PageStatusModelDto page = new();
            page.Id = dataState.Pages.Last().Id + 1;
            dataState.Pages.Add(page);
            dataState.AvailablePageId = page.Id;
            MetaDataManeger.MetaDataHasChanged(dataState);
            AddPageInFile(DataPagesBasePath + dataState.AvailablePageId);
            return dataState.Pages.Last().AvailableSpace;
        }

        public static void UpdatePage<Thing>(PageStatusModelDto page, AddItemsModelDto<Thing> addItems)
        {
            string path = DataPagesBasePath + page.Id;
            WriteItemsInFile(path,addItems);
        }

        public static void ClearAllPages()
        {
            DirectoryInfo dir = new DirectoryInfo(DataPagesBasePath);
            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }
            MetaDataManeger.ClearMetaData();
        }

        private static int GetId<Thing>(this Thing item)
        {
            if (item == null)
                throw new Exception("things is null");

            var idProperty = item.GetType().GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                object? Id = idProperty.GetValue(item);
                if (Id != null)
                    return (int)Id;
                else throw new Exception("object not contain Id Property");

            }
            else
                throw new Exception("object not contain Id Property");
        }
    }
}
