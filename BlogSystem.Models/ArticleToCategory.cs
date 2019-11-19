using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BlogSystem.Models
{
    public class Article : BaseEntity
    {
        /// <summary>
        /// 文章表
        /// </summary>
        [Required]
        public string Title { get; set; }
        [Column(TypeName = "ntext"), Required]
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public int GoodConut { get; set; }

        public int BadCount { get; set; }


    }
}
