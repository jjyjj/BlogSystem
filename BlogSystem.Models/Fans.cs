using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace BlogSystem.Models
{
    public class Fans : BaseEntity
    {
        /// <summary>
        /// 粉丝表
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(FocusUser))]
        public Guid FocusUserId { get; set; }
        public User FocusUser { get; set; }
    }
}
