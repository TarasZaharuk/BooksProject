﻿using BooksProject.Shared;

namespace Services
{
    public class BooksGenerator
    {
        private List<BookDetailsDto>? Books = [];
        private static List<string> _bookNames = new List<string>
        {
            "The Shadow of the Wind",
            "The Great Gatsby",
            "Harry Potter and the Sorcerer's Stone",
            "To Kill a Mockingbird",
            "1984",
            "The Catcher in the Rye",
            "Pride and Prejudice",
            "The Lord of the Rings",
            "Animal Farm",
            "The Hobbit",
            "Brave New World",
            "The Da Vinci Code",
            "The Alchemist",
            "The Book Thief",
            "Lord of the Flies",
            "Gone with the Wind",
            "The Chronicles of Narnia",
            "The Hitchhiker's Guide to the Galaxy",
            "The Grapes of Wrath",
            "Moby-Dick",
            "One Hundred Years of Solitude",
            "Wuthering Heights",
            "Crime and Punishment",
            "Jane Eyre",
            "The Picture of Dorian Gray",
            "The Adventures of Huckleberry Finn",
            "Les Misérables",
            "A Tale of Two Cities",
            "Anna Karenina",
            "The Brothers Karamazov",
            "The Count of Monte Cristo",
            "The Odyssey",
            "The Iliad",
            "The Divine Comedy",
            "Don Quixote",
            "War and Peace",
            "Frankenstein",
            "Dracula",
            "The Adventures of Sherlock Holmes",
            "The Three Musketeers",
            "Treasure Island",
            "Alice's Adventures in Wonderland",
            "Through the Looking-Glass",
            "Moby-Dick",
            "The Adventures of Tom Sawyer",
            "Adventures of Huckleberry Finn",
            "The Call of the Wild",
            "White Fang",
            "Walden",
            "The Red Badge of Courage",
            "The Scarlet Letter",
            "The Wonderful Wizard of Oz",
            "The Secret Garden",
            "Little Women",
            "Alice's Adventures in Wonderland",
            "Through the Looking-Glass",
            "The Wind in the Willows",
            "Anne of Green Gables",
            "A Little Princess",
            "The Jungle Book",
            "Peter Pan",
            "The Wizard of Oz",
            "The War of the Worlds",
            "Journey to the Center of the Earth",
            "Twenty Thousand Leagues Under the Sea",
            "Around the World in Eighty Days",
            "The Time Machine",
            "The Island of Dr. Moreau",
            "The Invisible Man",
            "The First Men in the Moon",
            "The War of the Worlds",
            "The Lost World",
            "Frankenstein",
            "Dracula",
            "The Strange Case of Dr. Jekyll and Mr. Hyde",
            "The Phantom of the Opera",
            "The Turn of the Screw",
            "The Legend of Sleepy Hollow",
            "The Tell-Tale Heart",
            "The Call of Cthulhu",
            "At the Mountains of Madness",
            "The Shadow over Innsmouth",
            "The Dunwich Horror",
            "The Colour Out of Space",
            "The Case of Charles Dexter Ward",
            "The Thing on the Doorstep",
            "The Whisperer in Darkness",
            "The Dreams in the Witch House",
            "The Haunter of the Dark",

        };
        private static List<string> _authors = new List<string>
        {
            "Jane Austen",
            "Fyodor Dostoevsky",
            "Charles Dickens",
            "Leo Tolstoy",
            "George Orwell",
            "J.K. Rowling",
            "Mark Twain",
            "Ernest Hemingway",
            "William Shakespeare",
            "Emily Brontë",
            "Herman Melville",
            "Gabriel García Márquez",
            "Charlotte Brontë",
            "George Eliot",
            "Miguel de Cervantes",
            "James Joyce",
            "J.R.R. Tolkien",
            "John Steinbeck",
            "Victor Hugo",
            "T.S. Eliot",
            "H.G. Wells",
            "Thomas Hardy",
            "Arthur Conan Doyle",
            "H.P. Lovecraft",
            "Agatha Christie",
            "Stephen King",
            "Toni Morrison",
            "Harper Lee",
            "Ray Bradbury",
            "Aldous Huxley",
            "Philip K. Dick",
            "Jules Verne",
            "C.S. Lewis",
            "Kurt Vonnegut",
            "William Faulkner",
            "Dante Alighieri",
            "Emily Dickinson",
            "Edgar Allan Poe",
            "Virginia Woolf",
            "George R.R. Martin",
            "Roald Dahl",
            "Ken Follett",
            "Terry Pratchett",
            "Johann Wolfgang von Goethe",
            "Alexandre Dumas",
            "Margaret Atwood",
            "Albert Camus",
            "Kazuo Ishiguro",
            "Yukio Mishima",
            "Milan Kundera",
            "Hermann Hesse",
            "James Baldwin",
            "Albert Einstein",
            "Neil Gaiman",
            "Hilary Mantel",
            "Donna Tartt",
            "Zadie Smith",
            "Haruki Murakami",
            "David Foster Wallace",
            "Julian Barnes",
            "D.H. Lawrence",
            "Ernest Hemingway",
            "Sylvia Plath",
            "Langston Hughes",
            "Gabriel García Márquez",
            "Marcel Proust",
            "Leo Tolstoy",
            "Virginia Woolf",
            "Margaret Atwood",
            "Aldous Huxley",
            "Franz Kafka",
            "Vladimir Nabokov",
            "John Steinbeck",
            "Kurt Vonnegut",
            "Gabriel García Márquez",
            "E.M. Forster",
            "Harper Lee",
            "Daphne du Maurier",
            "J.D. Salinger",
            "Walt Whitman",
            "Charlotte Brontë",
            "Fyodor Dostoevsky",
            "Thomas Hardy",
            "Herman Melville",
            "Emily Brontë",
            "J.R.R. Tolkien",
            "Leo Tolstoy",
            "Toni Morrison",
            "Albert Camus",
            "H.G. Wells",
            "Jane Austen",
            "Margaret Atwood",
            "Gabriel García Márquez",
            "Virginia Woolf",
            "Aldous Huxley",
            "Haruki Murakami",

        };

        public List<BookDetailsDto> GenerateBooksList(int lastId,int generateBoooksCount, bool InitializeId)
        {
            if (generateBoooksCount > 10000)
            {
                generateBoooksCount = 10000;
            }
            Books = [];
            Random random = new();
            for (int i = 0; i < generateBoooksCount; i++)
            {
                BookDetailsDto book = new()
                {
                    Name = _bookNames[random.Next(0, _bookNames.Count)],
                    Author = _authors[random.Next(0, _authors.Count)],
                    Description = $"Description{i}",
                    DateOfPublishing = DateOnly.FromDateTime(DateTime.Now),
                    Rating = random.Next(0, 6),
                    Status = Status.Draft
                };
                Books ??= [];
                if (InitializeId)
                {
                    if (Books.Count != 0)
                        book.Id = Books.Last().Id + 1;
                    else
                        book.Id = lastId + 1;
                }
                else
                {
                    book.Id = 0;
                }
                Books.Add(book);
            }

            return Books;
        }
    }
}
