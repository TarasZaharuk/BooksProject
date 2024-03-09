using BooksProject.Shared;
using BooksWebApi.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BooksWebApi.Repositories
{
    public class InMemoryBooksRepository : IBooksRepository
    {

        public GetBooksListModelDto? GetAll(int skipBooks, int takeBooks, bool sortAscending, bool sortDescending, string? searchBook)
        {
            GetBooksListModelDto getBooksList = new GetBooksListModelDto();

            if (sortAscending == false && sortDescending == false && string.IsNullOrWhiteSpace(searchBook))
            {
                getBooksList.TotalCount = BooksContainer.Books.Count;
                getBooksList.Books = BooksContainer.Books.Skip(skipBooks).Take(takeBooks);

                return getBooksList;
            }

            if (string.IsNullOrWhiteSpace(searchBook))
            {
                if (sortAscending)
                {
                    IEnumerable<BookDetailsDto> sortedList = BooksContainer.Books.OrderBy(book => book.Name);
                    getBooksList.TotalCount = sortedList.ToList().Count;
                    getBooksList.Books = sortedList.Skip(skipBooks).Take(takeBooks);

                    return getBooksList;
                }

                else if (sortDescending)
                {
                    IEnumerable<BookDetailsDto> sortedList = BooksContainer.Books.OrderByDescending(book => book.Name);
                    getBooksList.TotalCount = sortedList.ToList().Count;
                    getBooksList.Books = sortedList.Skip(skipBooks).Take(takeBooks);

                    return getBooksList;
                }
            }

            else if (!string.IsNullOrWhiteSpace(searchBook) && sortAscending == false && sortDescending == false)
            {
                IEnumerable<BookDetailsDto> searchList = BooksContainer.Books;

                searchList = searchList.Where(element =>
                {
                    if (element.Name.Contains(searchBook, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                }).ToArray();
                getBooksList.TotalCount = searchList.ToList().Count;
                getBooksList.Books = searchList.Skip(skipBooks).Take(takeBooks);

                return getBooksList;
            }

            else if (!string.IsNullOrWhiteSpace(searchBook) && sortDescending)
            {
                IEnumerable<BookDetailsDto> searchList = BooksContainer.Books;

                searchList = searchList.Where(element =>
                {
                    if (element.Name.Contains(searchBook, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                }).ToArray();
                getBooksList.TotalCount = searchList.ToList().Count;
                IEnumerable<BookDetailsDto> sortedSearchedList = searchList.OrderByDescending(book => book.Name);

                getBooksList.Books = sortedSearchedList.Skip(skipBooks).Take(takeBooks);

                return getBooksList;
            }

            else if (!string.IsNullOrWhiteSpace(searchBook) && sortAscending)
            {
                IEnumerable<BookDetailsDto> searchList = BooksContainer.Books;

                searchList = searchList.Where(element =>
                {
                    if (element.Name.Contains(searchBook, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                }).ToArray();
                getBooksList.TotalCount = searchList.ToList().Count;
                IEnumerable<BookDetailsDto> sortedSearchedList = searchList.OrderBy(book => book.Name);

                getBooksList.Books = sortedSearchedList.Skip(skipBooks).Take(takeBooks);

                return getBooksList;
            }

            return null;
        }

        public BookDetailsDto? GetById(int id)
        {
            BookDetailsDto? searchedBook = BooksContainer.Books.SingleOrDefault(book => book.Id == id);
            return searchedBook;
        }

        public void DeleteBook(int id)
        {
            BookDetailsDto? book = BooksContainer.Books.SingleOrDefault(book => book.Id == id);
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

            if (string.IsNullOrWhiteSpace(adedBook.Description))
            {
                adedBook.Description = $"{adedBook.Name}-{adedBook.Author}-{adedBook.DateOfPublishing.ToShortDateString()}";
            }
            book.Name = adedBook.Name;
            book.Description = adedBook.Description;
            book.Author = adedBook.Author;
            book.DateOfPublishing = DateOnly.FromDateTime(adedBook.DateOfPublishing);
            book.Rating = adedBook.Rating;
            book.Status = Status.Draft;

            BooksContainer.Books.Add(book);

            return book.Id;
        }

    }
}
