using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using DataBaseManeger.Abstractions;
using DataBaseModels;
using System.Threading.Tasks;

namespace DataBaseManeger
{
    internal class DataBaseManeger : IDataBaseManeger
    {
        private static string DataBaseInfoPath { get; set; } = null!;
        private static string DataBasePagePath { get; set; } = null!;

        public void SetPath(string path)
        {
            string dataBaseDirectory = path + "DataBase\\";
            string dataBasePagesDirectory = dataBaseDirectory + "Pages\\";
            string dataBaseInfoDirectory = dataBaseDirectory + "Info";
            if (!Directory.Exists(dataBaseDirectory))
            Directory.CreateDirectory(dataBaseDirectory);
            if (!Directory.Exists(dataBasePagesDirectory))
            Directory.CreateDirectory(dataBasePagesDirectory);
            if (!File.Exists(dataBaseInfoDirectory))
            {
                using (StreamWriter streamWriter = File.CreateText(dataBaseInfoDirectory))
                {
                    streamWriter.Write(string.Empty);
                }
            }
            
            
            DataBaseInfoPath = dataBaseInfoDirectory;
            DataBasePagePath = dataBasePagesDirectory;
        }

        public string GetDataPagesPath()
        {
            return DataBasePagePath;
        }
        public void MetaDataHasChanged(PageStatusModel pageStatus, PageStatusModel newPageStatus)
        {
            DataBaseStatusModel dataBaseStatus = GetDataBaseMetaData();
            PageStatusModel page = dataBaseStatus.Pages.SingleOrDefault(page => page.Id == pageStatus.Id) ?? throw new Exception("page is null");

            int index = dataBaseStatus.Pages.IndexOf(page);
            dataBaseStatus.Pages[index] = newPageStatus;
            ChangeMeta(dataBaseStatus);
        }

        public void ClearMetaData()
        {
            using (StreamWriter streamWriter = new(DataBaseInfoPath))
            {
                streamWriter.Write(string.Empty);
            }
        }

        public DataBaseStatusModel GetDataBaseMetaData()
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

            DataBaseStatusModel? dataState = JsonSerializer.Deserialize<DataBaseStatusModel>(dataSateJsonText);

            return dataState ?? throw new Exception("DataBase Status is null");
        }

        public int GetLastId()
        {
            DataBaseStatusModel dataBaseMeta = GetDataBaseMetaData();
            dataBaseMeta = GetDataBaseMetaData();
            return dataBaseMeta.LastId;
        }

        public int GetTotalCount()
        {
            DataBaseStatusModel dataBaseStatus = GetDataBaseMetaData();

            int totalCount = 0;

            foreach (var page in dataBaseStatus.Pages)
            {
                totalCount += page.CountOfItems;
            }

            return totalCount;
        }

        public void MetaDataHasChanged(PageStatusModel pageStatus)
        {
            DataBaseStatusModel dataBaseStatus = GetDataBaseMetaData();
            dataBaseStatus.FirstId = dataBaseStatus.Pages.First().Id;
            dataBaseStatus.LastId = pageStatus.LastId;

            dataBaseStatus.Pages.Last().LastId = pageStatus.LastId;
            dataBaseStatus.Pages.Last().FirstId = pageStatus.FirstId;
            dataBaseStatus.Pages.Last().CountOfItems = pageStatus.CountOfItems;

            ChangeMeta(dataBaseStatus);
        }

        private void CreateMetaData()
        {
            PageStatusModel page = new();
            DataBaseStatusModel dataStatus = new DataBaseStatusModel
            {
                Pages = new List<PageStatusModel> { page },
                AvailablePage = page,
            };
            ChangeMeta(dataStatus);

        }


        private void AddPageInFile(PageStatusModel page)
        {
            string path = DataBasePagePath + page.Id;
            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(string.Empty);
            }
        }

        private DataBaseStatusModel CreatePage(DataBaseStatusModel dataBaseStatus)
        {
            DataBaseStatusModel dataState = dataBaseStatus;
            PageStatusModel page = new();
            page.Id = dataState.Pages.Last().Id + 1;
            dataState.Pages.Add(page);
            dataState.AvailablePage = page;
            return dataState;
        }

        private void WriteMetaDataInFile(DataBaseStatusModel dataStatus)
        {
            using (StreamWriter streamWriter = new(DataBaseInfoPath))
            {
                streamWriter.Write(JsonSerializer.Serialize(dataStatus));
            }
        }

        private void ChangeMeta(DataBaseStatusModel dataBaseStatus)
        {
            if (!File.Exists(DataBasePagePath + dataBaseStatus.Pages.Last().Id))
            {
                AddPageInFile(dataBaseStatus.Pages.Last());
            }
                

            if (dataBaseStatus.Pages.Last().AvailableSpace == 0)
            {
                dataBaseStatus = CreatePage(dataBaseStatus);
                AddPageInFile(dataBaseStatus.Pages.Last());
            }
                
            WriteMetaDataInFile(dataBaseStatus);
        }

    }
}