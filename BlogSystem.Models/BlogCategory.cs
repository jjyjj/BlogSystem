using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace BlogSystem.Models
{
    public class BlogCategory : BaseEntity
    {
        /// <summary>
        /// 博客类别表
        /// </summary>
        public string Category { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
