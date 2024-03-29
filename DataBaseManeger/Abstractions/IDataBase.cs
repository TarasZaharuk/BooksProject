using BooksProject.Shared;
using DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManeger.Abstractions
{
    public interface IDataBase<T>
    {
        T InitializeId(T item);

        List<T> GetAll();

        GetBooksListModelDto<T> GetAll(GetListRequestDto getListRequest);

        T? GetById(int id);

        void DeleteAll();

        void DeleteItem(int id);

        void AddItem(T addItem);

        void AddItems(List<T> addItems);

        int GetLastId();
    }
}
