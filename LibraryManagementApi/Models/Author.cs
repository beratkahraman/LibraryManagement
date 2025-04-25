using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
