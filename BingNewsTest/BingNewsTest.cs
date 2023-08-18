using System.Drawing;
using System;
using System.Xml.Linq;

namespace BingNewsTest
{
    public class BingNewsTest
    {

        [Fact]
        public void Init_Service()
        {
            var bingNewService = new BingNewService();
            
            Assert.NotNull(bingNewService);
        }

        [Fact] public void Create_New_BasePost()
        {
            var bingNewService = new BingNewService();
            var postedTime = DateTime.Now;
            var title = "Newspaper";
            var provider = bingNewService.CreateProvider("provider Icon", "provider Name");

            var post = bingNewService.CreateBasePost(provider, postedTime, title);

            Assert.NotNull(post);
            Assert.Equal(provider.GetProviderIcon(), post.GetPoviderIcon());
            Assert.Equal(provider.GetProviderName(), post.GetProviderName());
            Assert.Equal(postedTime.ToString(), post.GetPostedTime());
            Assert.Equal(title, post.GetTitle());
        }

        [Fact]
        public void Create_New_Article()
        {
            var bingNewService = new BingNewService();
            var image = "src";
            var postedTime = DateTime.Now;
            var title = "Newspaper";
            var likeNumber = 4;
            var disLikeNumber = 5;
            var commentNumber = 8;
            var provider = bingNewService.CreateProvider("provider Icon", "provider Name");

            var article = bingNewService.CreateArticle(image, postedTime, provider, title, likeNumber, disLikeNumber, commentNumber);

            Assert.NotNull(article);
            Assert.Equal(title, article.GetTitle());
            Assert.Equal(image, article.GetImage());
            Assert.Equal(postedTime.ToString(), article.GetPostedTime());
            Assert.Equal(provider.GetProviderIcon(), article.GetPoviderIcon());
            Assert.Equal(provider.GetProviderName(), article.GetProviderName());
            Assert.Equal(likeNumber, article.GetLikeNumber());
            Assert.Equal(disLikeNumber, article.GetDislikeNumber());
            Assert.Equal(commentNumber, article.GetCommentNumber());
        }

        [Fact]
        public void Create_New_AdArticle()
        {
            var bingNewService = new BingNewService();
            var title = "title";
            var image = "_image";
            var postedTime = DateTime.Now;
            var provider = bingNewService.CreateProvider("pIcon", "pName");

            var adArticle = bingNewService.CreateAdArticle(image, title, postedTime, provider);

            Assert.NotNull(adArticle);
            Assert.Equal(title, adArticle.GetTitle());
            Assert.Equal(image, adArticle.GetImage());
            Assert.Equal(postedTime.ToString(), adArticle.GetPostedTime());
            Assert.Equal(provider.GetProviderIcon(), adArticle.GetPoviderIcon());
            Assert.Equal(provider.GetProviderName(), adArticle.GetProviderName());
        }

        [Fact]
        public void Create_New_Microsoft_Notification()
        {
            var bingNewService = new BingNewService();
            var topic = "title";
            var image = "src";
            var type = TypeOfNotification.Comment;
            var noticeFrom = "some one";

            var notice = bingNewService.CreateMicrosoftNotification(topic, image, type, noticeFrom);

            Assert.NotNull(notice);
            Assert.Equal(topic, notice.GetTopic());
            Assert.Equal(image, notice.GetImage());
            Assert.Equal(type, notice.GetTypes());
            Assert.Equal(noticeFrom, notice.GetNoticeFrom());
        }

        [Fact]
        public void Create_Pagination()
        {
            var bingNewService = new BingNewService();
            var total = 10;
            var index = 0;
            var size  = 1;
            var pageNumber = (int)Math.Ceiling(total * 1.0 / size);

            var pagination = bingNewService.CreatePagination(total, index, size);

            Assert.NotNull(pagination);
            Assert.Equal(total, pagination.GetTotal());
            Assert.Equal(index, pagination.GetIndex());
            Assert.Equal(size, pagination.GetSize());
            Assert.Equal(pageNumber, pagination.GetPageNumber());
        }

        [Fact]
        public void Create_Article_Paging()
        {
            var bingNewService = new BingNewService();
            var pagination = bingNewService.CreatePagination(9,1,3);

            var pageResult = bingNewService.CreatePagedResult(pagination);

            Assert.NotNull(pageResult);
            Assert.Equal(3, pageResult.Items.Count);
        }
    }
}