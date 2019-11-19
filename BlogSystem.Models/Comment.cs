using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Models
{
    public class Comment : BaseEntity
    {
        /// <summary>
        /// 评论表
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(maximumLength: 800)]
        public string Content { get; set; }

        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
