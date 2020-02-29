using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Models
{
    public class User : BaseEntity
    {
        /// <summary>
        /// 用户表
        /// </summary>
        [Required, StringLength(maximumLength: 40), Column(TypeName = "varchar")]
        public string Email { get; set; }
        [Required, StringLength(maximumLength: 300), Column(TypeName = "varchar")]
        public string Password { get; set; }
        [Required, StringLength(maximumLength: 300), Column(TypeName = "varchar")]
        public string ImagePath { get; set; }
        public string Motto { get; set; }
        public int type { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FocusCount { get; set; }
        /// <summary>
        /// 网站名
        /// </summary>
        public string SiteName { get; set; }
    }
}
