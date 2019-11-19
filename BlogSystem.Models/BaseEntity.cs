using System;

namespace BlogSystem.Models
{
    public class BaseEntity
    {
        /// <summary>
        /// 基本类型
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsRemoved { get; set; }
    }
}
