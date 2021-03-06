﻿using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class ArticleManager : IArticleManager
    {


        public async Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId)
        {
            using (var articleSvc = new ArticleService())
            {
                var article = new Article()
                {
                    Title = title,
                    Content = content,
                    UserId = userId
                };
                await articleSvc.CreateAsync(article);

                Guid articleId = article.Id;
                using (var articleToCategorySvc = new ArticleToCategoryService())
                {
                    foreach (var categoryId in categoryIds)
                    {
                        await articleToCategorySvc.CreateAsync(new ArticleToCategory()
                        {
                            ArticleId = articleId,
                            BlogCategoryId = categoryId
                        }, saved: false);
                    }

                    await articleToCategorySvc.Save();

                }
            }

        }


        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public async Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(false, articleId);
                article.Title = title;
                article.Content = content;
                await articleService.EditAsync(article);
                using (IDAL.IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var categoryId in articleToCategoryService.GetAllAsync(false).Where(m => m.ArticleId == articleId))
                    {
                        await articleToCategoryService.RemoveAsync(categoryId, false);
                    }

                    foreach (var categroyId in categoryIds)
                    {
                        await articleToCategoryService.CreateAsync(new ArticleToCategory()
                        {
                            ArticleId = articleId,
                            BlogCategoryId = categroyId
                        }, false);
                    }
                    await articleToCategoryService.Save();

                }
            }
        }
        /// <summary>
        /// 编辑文章类别
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="newCategoryName"></param>
        /// <returns></returns>


        public async Task<bool> ExistsArticle(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync(false).AnyAsync(m => m.Id == articleId);
            }

        }

        /// <summary>
        /// 根据文章类别查找文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据用户Email查找文章
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<List<ArticleDto>> GetAllArticlesByEmail(string email)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查找当前用户所有文章
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize)
        {
            using (var articleService = new ArticleService())
            {
                var list = await articleService
                    .GetAllByPageOrderAsync(false, pageSize, pageIndex, false)

                    .Include(m => m.User)
                    .Where(m => m.UserId == userId)
                    .Select(m => new ArticleDto()
                    {
                        Title = m.Title,
                        Content = m.Content,
                        GoodConut = m.GoodConut,
                        Eamil = m.User.Email,
                        CreateTime = m.CreateTime,
                        BadCount = m.BadCount,
                        Id = m.Id,
                        ImagePath = m.User.ImagePath,
                        BrowseCount = m.BrowseCount

                    }).ToListAsync();
                foreach (var item in list)
                {
                    item.Content = CommonMethods.SplitString(item.Content, 150, "...");
                }
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var articleDto in list)
                    {
                        var cates = await articleToCategoryService
                            .GetAllAsync(false).Include(m => m.BlogCategory)
                            .Where(m => m.ArticleId == articleDto.Id)
                            .ToListAsync();
                        articleDto.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
                        articleDto.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();
                    }
                    return list;
                }

            }
        }

        public Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<ArticleDto>> GetAllArticles(bool all, int pageIndex, int pageSize)
        {
            using (var articleService = new ArticleService())
            {
               

                var list = await articleService
                    .GetAllByPageOrderAsync(all, pageSize, pageIndex, false)
               
                    .Select(m => new ArticleDto()
                    {
                        Title = m.Title,
                        Content = m.Content,
                        GoodConut = m.GoodConut,
                        Eamil = m.User.Email,
                        CreateTime = m.CreateTime,
                        BadCount = m.BadCount,
                        Id = m.Id,
                        Name = m.User.SiteName,
                        ImagePath = m.User.ImagePath,
                        TimeInterval = "null",
                        ArticleImgUrls = "null",
                        BrowseCount = m.BrowseCount
                    }).ToListAsync();

                foreach (var item in list)
                {
                    string[] ss = CommonMethods.GetHtmlImageUrlList(item.Content);

                    item.ArticleImgUrls = ss[0];
                    item.Content = CommonMethods.SplitString(item.Content, 45, "...");
                    item.TimeInterval = CommonMethods.ComputeTime(item.CreateTime);

                }
                return list;
            }
        }




        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task RemoveArticle(Guid articleId, bool isConfirm)
        {
            BlogContext blogContext = new BlogContext();



            using (IDAL.IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
            {
                var articleToCategories = articleToCategoryService.GetAllAsync(false).Where(m => m.ArticleId == articleId);

                foreach (var articleToCategoryId in articleToCategories)
                {
                    if (isConfirm)
                    {
                        blogContext.Entry(articleToCategoryId).State = EntityState.Deleted;
                    }
                    else
                    {
                        await articleToCategoryService.RemoveAsync(articleToCategoryId, saved: false);
                        
                    }
                }

                using (IDAL.IArticleService articleService = new ArticleService())
                {
                    if (isConfirm)
                    {
                        blogContext.Entry(new Models.Article { Id = articleId }).State = EntityState.Deleted;
                    }
                    else
                    {
                        await articleService.RemoveAsync(articleId, saved: false);
                        await articleService.Save();

                    }


                    await blogContext.SaveChangesAsync();

                }

            }
        }

        //删除文章类别
        public async Task RemoveCategory(Guid categoryId)
        {

            IDAL.IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService();
            await articleToCategoryService.RemoveAsync(categoryId, false);
            await articleToCategoryService.Save();

        }

        //获取一篇文章详情
        public async Task<ArticleDto> GetOneArticleById(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var data = await articleService.GetAllAsync(false)
                    .Include(m => m.User)//联表查询
                    .Where(m => m.Id == articleId)//查询条件
                    .Select(m => new Dto.ArticleDto()
                    {
                        Id = m.Id,
                        BadCount = m.BadCount,
                        Title = m.Title,
                        Content = m.Content,
                        CreateTime = m.CreateTime,
                        Eamil = m.User.Email,
                        GoodConut = m.GoodConut,
                        ImagePath = m.User.ImagePath,
                        Name = m.User.SiteName,
                        BrowseCount = m.BrowseCount

                    }).FirstAsync();//取出该条数据
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    var cates = await articleToCategoryService
                        .GetAllAsync(false)
                        .Include(m => m.BlogCategory)
                        .Where(m => m.ArticleId == data.Id)
                        .ToListAsync();
                    data.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
                    data.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();
                    return data;
                }

            }
        }
        //获取总页码
        public async Task<int> GetDataCount(Guid UserId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync(false).CountAsync(m => m.UserId == UserId);
            }
        }
        //获取当前文章的评论总数量
        public async Task<int> GetCommentsForArticleCountByArticleId(Guid ArticleId)
        {

            using (IDAL.ICommentService commentService = new CommentService())
            {
                return await commentService.GetAllAsync(false).CountAsync(m => m.ArticleId == ArticleId);

            }
        }

        public async Task<int> GetDataCount()
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync(false).CountAsync();
            }
        }
        //点赞
        public async Task GoodCountAdd(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(false, articleId);
                article.GoodConut++;
                await articleService.EditAsync(article);

            }
        }

        //反对
        public async Task BadCountAdd(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(false, articleId);
                article.BadCount++;
                await articleService.EditAsync(article);

            }
        }
        #region 评论
        //评论
        public async Task CreateComment(Guid userId, Guid articleId, string content)
        {


            using (IDAL.ICommentService commentService = new CommentService())
            {
                await commentService.CreateAsync(new Comment()
                {
                    UserId = userId,
                    ArticleId = articleId,
                    Content = content
                });
            }
        }
        //查询评论
        public async Task<List<Dto.CommentDto>> GetCommentsByArticleId(Guid articleId)
        {
            using (IDAL.ICommentService commentService = new CommentService())
            {
                return await commentService.GetAllOrderAsync(false, asc: false)
                     .Where(m => m.ArticleId == articleId)
                     .Include(m => m.User)
                     .Select(m => new Dto.CommentDto()
                     {
                         Id = m.Id,
                         ArticleId = m.ArticleId,
                         Content = m.Content,
                         CreateTime = m.CreateTime,
                         UserId = m.UserId,
                         Email = m.User.Email,
                         Name = m.User.SiteName

                     }).ToListAsync();
            }
        }


        //根据用户id获取所有评论      
        public async Task<List<Dto.CommentDto>> GetCommentsByUsersId(Guid usersId,string str)
        {
          
            using (IDAL.ICommentService commentService = new CommentService())
            {
                List<CommentDto> list = new List<CommentDto>();
                if (str!=null)
                {
                    str = str.Trim();
                    list = await commentService.GetAllOrderAsync(false, asc: false)
                   .Where(m => m.UserId == usersId)
                   .Where(m=>m.Article.Title.Contains(str))
                   .Include(m => m.User)
                   .Select(m => new Dto.CommentDto()
                   {
                       Id = m.Id,
                       ArticleId = m.ArticleId,
                       Content = m.Content,
                       CreateTime = m.CreateTime,
                       Name = m.User.SiteName
                   }).ToListAsync();
                }
                else
                {
                    list = await commentService.GetAllOrderAsync(false, asc: false)
                   .Where(m => m.UserId == usersId)
                   .Include(m => m.User)
                   .Select(m => new Dto.CommentDto()
                   {
                       Id = m.Id,
                       ArticleId = m.ArticleId,
                       Content = m.Content,
                       CreateTime = m.CreateTime,
                       Name = m.User.SiteName
                   }).ToListAsync();
                }
              
                using (IArticleService articleService = new ArticleService())
                {
                    foreach (var commentDto in list)
                    {
                        var cates = await articleService
                            .GetAllAsync(false)
                            .Where(m => m.Id == commentDto.ArticleId)
                            .ToListAsync();

                        commentDto.ArticleTitles = cates.Select(m => m.Title).ToArray();
                    }
                    return list;
                }
            }
        }
        public async Task RemoveComment(Guid commentId)
        {
            BlogContext blogContext = new BlogContext();
            using (IDAL.ICommentService commentService = new CommentService())
            {

                blogContext.Entry(new Models.Comment { Id = commentId }).State = EntityState.Deleted;
                await commentService.Save();

                await blogContext.SaveChangesAsync();



            }
        }
        public async Task BrowseCountAdd(Guid articleId)
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var article = await articleService.GetOneByIdAsync(false, articleId);
                article.BrowseCount++;
                await articleService.EditAsync(article);

            }
        }
        /// <summary>
        /// 获取到当前用户文章中浏览最高的
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task<List<Dto.ArticleDto>> GetHotArticleByUsersId(Guid usersId)
        {


            using (IDAL.IArticleService articleService = new ArticleService())
            {
                var list = await articleService.GetAllAsync(false)
                    .Include(m => m.User)
                     .Where(m => m.UserId == usersId)
                   .OrderByDescending(m => m.BrowseCount)
                    .Take(3)

                    .Select(m => new Dto.ArticleDto()
                    {
                        Id = m.Id,
                        Title = m.Title,

                    }).ToListAsync();
                return list;
            }
        }
        /// <summary>
        /// 获取到最新被评论的博客
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task<List<Article>> GetNewArticleForCommentByUserId(Guid usersId)
        {



            using (IDAL.ICommentService commentService = new CommentService())
            {
                var comments = await commentService.GetAllAsync(false)

                    .OrderByDescending(m => m.CreateTime)
                    .Take(3)
                    .Select(m => new Dto.CommentDto()
                    {
                        Id = m.Id,
                        ArticleId = m.ArticleId,
                        Content = m.Content,
                        CreateTime = m.CreateTime,
                        Name = m.User.SiteName
                    }).ToListAsync();
                var articleResult = new List<Article>();
                using (IDAL.IArticleService articleService = new ArticleService())
                {
                 

                    foreach (var item in comments)
                    {
                        var articles = await articleService.GetAllAsync(false)

                             .Where(c => c.Id == item.ArticleId && c.UserId == usersId)
                          
                             .ToListAsync();
                        var articleArray = articles.Select(m => new Article()
                        {
                            Id = m.Id,
                            Title = m.Title
                        }).ToArray();
                        for (int i = 0; i < articleArray.Length; i++)
                        {
                            articleResult.Add(articleArray[i]);
                        }
                      
                    }
                    return articleResult;
                }

            }





        }
    }
}

#endregion

