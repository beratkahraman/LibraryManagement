using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryManagementApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int PublishedYear { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int AuthorId { get; set; }

        [JsonIgnore]
        public Author? Author { get; set; }
    }
}