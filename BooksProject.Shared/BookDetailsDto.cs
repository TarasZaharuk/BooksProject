﻿using System;
using System.Data.Common;
using OdataToEntity;

namespace BooksProject.Shared
{
    public class BookDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? Author { get; set; }

        public DateTime DateOfPublishing { get; set; }

        public int Rating { get; set; }
    }
}
