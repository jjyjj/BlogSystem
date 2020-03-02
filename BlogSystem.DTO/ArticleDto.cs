using System;

namespace BlogSystem.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
        public string Eamil { get; set; }
        public int GoodConut { get; set; }
        public int BadCount { get; set; }
        public string ImagePath { get; set; }
        public string[] CategoryNames { get; set; }
        public Guid[] CategoryIds { get; set; }

        //时间差
        public string TimeInterval { get; set; }
        //评论数
        public int TotalComments { get; set; }
        //文章图片
        public string ArticleImgUrls { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int BrowseCount { get; set; }

    }
}
