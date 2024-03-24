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
        public static void AddBooks(List<BookDetailsDto> books)
        {
            do
            {
                if (PageManeger.GetLastPage().AvailableSpace == 0)
                    throw new Exception("AvailableSpace can not be null");

                List<BookDetailsDto> bookDetailsDtos = PageManeger.GetLastPageContent();
                bookDetailsDtos.AddRange(books.Take(PageManeger.GetLastPageAvailableSpace()));
                books = books.Skip(PageManeger.GetLastPageAvailableSpace()).ToList();
                WriteItemsInFile(bookDetailsDtos, DataPagesBasePath + PageManeger.GetLastPageId());
            } while (books.Count != 0);

        }

        public static void AddBook(BookDetailsDto book)
        {
            List<BookDetailsDto> books = new List<BookDetailsDto>{book};
            AddBooks(books);
        }

        private static void WriteItemsInFile(List<BookDetailsDto> books, string path)
        {
            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(JsonSerializer.Serialize(books));
            }

            MetaDataManeger.MetaDataHasChanged(books.Last().Id, books.Count, books.First().Id);
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

        public static void UpdatePage(List<BookDetailsDto> books, PageStatusModelDto page)
        {
            string path = DataPagesBasePath + page.Id;
            WriteItemsInFile(books, path);
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

    }
}
