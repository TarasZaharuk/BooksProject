using DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManeger.Abstractions
{
    internal interface IDataBaseManeger
    {
        DataBaseStatusModel GetDataBaseMetaData();

        void MetaDataHasChanged(PageStatusModel pageStatus);

        void MetaDataHasChanged(PageStatusModel priviousPagestatus, PageStatusModel newPageStatus);

        int GetLastId();

        int GetTotalCount();

        void ClearMetaData();

        void SetPath(string path);

        string GetDataPagesPath();
    }
}
