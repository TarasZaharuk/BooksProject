﻿using BooksProject.Shared;
using BooksWebApi.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BooksWebApi.Repositories
{
    public class InMemoryBooksRepository : IBooksRepository
    {

        public List<BookDetailsDto> GetAll()
        {
            return BooksContainer.Books;
        }

        public BookDetailsDto? GetById(int id)
        {
            BookDetailsDto? searchedBook = BooksContainer.Books.SingleOrDefault(book => book.Id == id);
            return searchedBook; 
        }

        public void DeleteBook(int id)
        {
            BookDetailsDto? book =  BooksContainer.Books.SingleOrDefault(book => book.Id == id);
            if (book != null)
                BooksContainer.Books.Remove(book);
        }

        public void DeleteAll()
        {
            BooksContainer.Books.Clear();
        }

        public int AddBook(BookAddModelDto adedBook)
        {
            var book = new BookDetailsDto();
            BooksContainer.InitializeId(book);

                book.Name = adedBook.Name;
                book.Description = adedBook.Description;
                book.Author = adedBook.Author;
                book.DateOfPublishing = adedBook.DateOfPublishing;
                book.Rating = adedBook.Rating;

            BooksContainer.Books.Add(book);

            return book.Id;
        }
    }
}
