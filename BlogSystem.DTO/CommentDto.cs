using BlogSystem.Models;
using System;

namespace BlogSystem.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid ArticleId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public string [] ArticleTitles { get; set; }
        public User User { get; set; }
     
    }
}
