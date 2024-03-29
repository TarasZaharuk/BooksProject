using BooksProject.Shared;
using DataBaseModels;

namespace DataBaseManeger.Abstractions
{
    internal interface IPageManeger<T>
    {
        PageStatusModel AddItem(T addItem, DataBaseStatusModel dataBaseStatus);

        AddItemsModel<T> AddItems(List<T> addItems, DataBaseStatusModel dataBaseStatus);
        
        void ClearAllPages();

        PageModifyModel DeleteItem(int id, DataBaseStatusModel dataBaseStatus);

        List<T> GetAllPagesContent(DataBaseStatusModel dataBaseStatus);

        List<T> GetPageContentInDiapazon(GetListRequestDto getListRequest, DataBaseStatusModel dataBaseStatus);

        void SetPath(string pathPage);

        T? GetById(int id, DataBaseStatusModel dataBaseStatus);
    }
}
