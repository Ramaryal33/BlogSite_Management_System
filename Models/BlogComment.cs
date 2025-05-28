﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MurkyPluse.Models
{
    public class BlogComment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [MaxLength(100, ErrorMessage = "User name cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; } = DateTime.Now;

        [Required]
        public string Content { get; set; }

        [ForeignKey("BlogPost")]
        public int PostId { get; set; }

        public BlogPost Post { get; set; }  // Changed from Posts to Post (singular)
    }
}