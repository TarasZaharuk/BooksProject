using System;
using System.Data.Common;

namespace BooksProject.Shared
{
    public class BookDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Author { get; set; }

        public DateOnly DateOfPublishing { get; set; }

        public int Rating { get; set; }

        public Status Status { get; set; }
        
        //public BookDetailsDto(CreateBookMode createBook) 
        //{
        //    Id = MetaDataManeger.GetLastId() + 1;
        //    DataState dataState = MetaDataManeger.GetDataBaseMetaData();
        //    dataState.LastId = Id;
        //    MetaDataManeger.MetaDataHasChanged(dataState);
        //}
    }
}
