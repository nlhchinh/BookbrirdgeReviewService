using Microsoft.AspNetCore.Http;
using ReviewDomain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewApplication.Models
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
        public int BookId { get; set; }
        public int StoreId { get; set; }
        [Required]
        public int OrderId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ImageUrl {  get; set; }
    }
    public class ReviewCreateRequest
    {
        public string UserId { get; set; } = default!;
        public int BookId { get; set; }
        public int StoreId { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }

        public string Comment { get; set; } = default!;
        public IFormFile? Image { get; set; }

    }
    public class ReviewCreateDto
    {
        public string UserId { get; set; } = default!;
        public int BookId { get; set; }
        public int StoreId { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = default!;
        public string? ImageUrl { get; set; }
    }
}