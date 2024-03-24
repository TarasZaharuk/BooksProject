using System.Text.Json;
using DataBaseModels.Shared;

namespace DBMeneger
{
    public static class MetaDataManeger
    {
        private static string DataBaseInfoPath { get; set; } = null!;

        public static void SetDataBaseInfoPath(string path)
        {
            DataBaseInfoPath = path;
        }
        public static DataBaseStatusModelDto GetDataBaseMetaData()
        {
            string dataSateJsonText;
            using (StreamReader streamReader = new StreamReader(DataBaseInfoPath))
            {
                dataSateJsonText = streamReader.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(dataSateJsonText))
            {
                CreateMetaData();
                using (StreamReader streamReader = new StreamReader(DataBaseInfoPath))
                {
                    dataSateJsonText = streamReader.ReadToEnd();
                }
            }

            DataBaseStatusModelDto? dataState = JsonSerializer.Deserialize<DataBaseStatusModelDto>(dataSateJsonText);

            return dataState ?? throw new Exception("DataBase Status is null");
        }
        private static void CreateMetaData()
        {
            PageStatusModelDto page = new();
            DataBaseStatusModelDto dataStatus = new DataBaseStatusModelDto
            {
                Pages = new List<PageStatusModelDto> { page },
                AvailablePageId = page.Id,
            };
            dataStatus.WriteMetaDataInFile();
        }

        public static void WriteMetaDataInFile(this DataBaseStatusModelDto dataStatus)
        {
            using (StreamWriter streamWriter = new(DataBaseInfoPath))
            {
                streamWriter.Write(JsonSerializer.Serialize(dataStatus));
            }
        }

        public static void MetaDataHasChanged(int lastId, int countOfAdedBooks, int firstId)
        {
            DataBaseStatusModelDto dataBaseStatus = GetDataBaseMetaData();
            PageStatusModelDto page = new();
            page.LastId = lastId;
            page.FirstId = firstId;
            page.Id = dataBaseStatus.Pages.Last().Id;
            page.CountOfItems += countOfAdedBooks;
            page.AvailableSpace -= countOfAdedBooks;

            dataBaseStatus.LastId = lastId;
            dataBaseStatus.FirstId = dataBaseStatus.Pages.First().Id;
            dataBaseStatus.Pages[dataBaseStatus.Pages.IndexOf(dataBaseStatus.Pages.Last())] = page;
            dataBaseStatus.WriteMetaDataInFile();
        }

        public static void MetaDataHasChanged(DataBaseStatusModelDto dataBaseStatus)
        {
            dataBaseStatus.WriteMetaDataInFile();
        }

        public static int GetLastId()
        {
            string dataSateJsonText;
            using (StreamReader streamReader = new StreamReader(DataBaseInfoPath))
            {
                dataSateJsonText = streamReader.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(dataSateJsonText))
                CreateMetaData();

            JsonDocument jsonDocument = JsonDocument.Parse(dataSateJsonText);
            JsonElement root = jsonDocument.RootElement;
            root.TryGetProperty("LastId", out JsonElement lastIdElement);
            lastIdElement.TryGetInt32(out int lastId);

            return lastId;
        }

        public static int GetTotalCount()
        {
            DataBaseStatusModelDto dataBaseStatus = GetDataBaseMetaData();
            if (dataBaseStatus.Pages == null)
                return 0;
            int totalCount = 0;

            foreach (var page in dataBaseStatus.Pages)
            {
                totalCount += page.CountOfItems;
            }

            return totalCount;
        }

        public static void ClearMetaData()
        {
            using (StreamWriter streamWriter = new(DataBaseInfoPath))
            {
                streamWriter.Write(string.Empty);
            }
        }
    }
}
