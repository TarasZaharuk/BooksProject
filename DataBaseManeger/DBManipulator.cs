using BooksProject.Shared;
using DataBaseManeger.Abstractions;
using DataBaseModels;
namespace DataBaseManeger
{
    public class DBManipulator<T> : IDataBase<T> where T : IGenericDBItemsType
    {
        private IDataBaseManeger _dataBaseMeta = new DataBaseManeger();
        private IPageManeger<T> _pageManeger = new PageManeger<T>();

        public void SetPath(string dataBasePath)
        {
            _dataBaseMeta.SetPath(dataBasePath);
            _pageManeger.SetPath(_dataBaseMeta.GetDataPagesPath());
        }
        public T InitializeId(T item)
        {
            item.Id = _dataBaseMeta.GetLastId() + 1;
            return item;
        }

        public List<T> GetAll()
        {
            return _pageManeger.GetAllPagesContent(_dataBaseMeta.GetDataBaseMetaData());
        }

        public GetBooksListModelDto<T> GetAll(GetListRequestDto getListRequest)
        {
            GetBooksListModelDto<T> getBooksList = new();

            List<T> items = _pageManeger.GetPageContentInDiapazon(getListRequest, _dataBaseMeta.GetDataBaseMetaData());
            
            getBooksList.Items = items;
            getBooksList.TotalCount = _dataBaseMeta.GetTotalCount();



            return getBooksList.TotalCount == 0 ? new GetBooksListModelDto<T>() : getBooksList;
        }

        public T? GetById(int id)
        {
            return _pageManeger.GetById(id, _dataBaseMeta.GetDataBaseMetaData());
        }

        public void DeleteAll()
        {
            _pageManeger.ClearAllPages();
            _dataBaseMeta.ClearMetaData();
        }

        public void DeleteItem(int id)
        {
            PageModifyModel modifiedPage = _pageManeger.DeleteItem(id, _dataBaseMeta.GetDataBaseMetaData());
            _dataBaseMeta.MetaDataHasChanged(modifiedPage.PreviousPageStatus, modifiedPage.NewPageStatus);
        }

        public void AddItem(T addItem)
        {
            DataBaseStatusModel dataBaseStatus = _dataBaseMeta.GetDataBaseMetaData();
            PageStatusModel pageStatus = _pageManeger.AddItem(addItem, dataBaseStatus);
            _dataBaseMeta.MetaDataHasChanged(pageStatus);
        }

        public void AddItems(List<T> addItems)
        {
            int newItemsCount = addItems.Count;
            do
            {
                DataBaseStatusModel dataBaseStatus = _dataBaseMeta.GetDataBaseMetaData();
                AddItemsModel<T> addedItems = _pageManeger.AddItems(addItems, dataBaseStatus);
                _dataBaseMeta.MetaDataHasChanged(addedItems.PageStatus);
                newItemsCount -= addedItems.CountOfAddedItems;
            } while (newItemsCount != 0);

        }

        public int GetLastId()
        {
            return _dataBaseMeta.GetLastId();
        }
    }
}
