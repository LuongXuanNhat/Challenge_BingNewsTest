
using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json;

public class NewsService
{
    private readonly double _viewCoefficient = 0.1;
    private readonly double _likeCoefficient = 0.25;
    private readonly double _disLikeCoefficient = 0.25;
    private readonly double _commentCoefficient = 0.4;

    public NewsService()
    {

    }

    public List<Article> GetDataByChannel(List<Article> data, string channel)
    {
        return data.Where(x => x.Channel.Equals(channel)).ToList() ?? new List<Article>();
    }

    public List<Article> GetTopArticles(List<Article> data, int articleNumber)
    {
        var articles = new List<Article>();
        foreach (var item in data)
        {
            if(GetArticleByDate(item))
            {
                articles.Add(AddData(item));
            }    
        }
        if(articles.Count < articleNumber) 
        {
            return articles.OrderByDescending(x => x.Score).ToList(); 
        }
        return articles.OrderByDescending(x=>x.Score).Take(articleNumber).ToList();
    }

    private Article AddData(Article item)
    {
        item.ViewNumber = new Random().Next(1, 101);
        item.CommentNumber = new Random().Next(1, 101);
        item.LikeNumber = new Random().Next(1, 101);
        item.DisLikeNumber = new Random().Next(1, 101);
        item.Score = GetScore(item);

        return item;
    }

    private double GetScore(Article item)
    {
        return Math.Round(_viewCoefficient * item.ViewNumber
            + _likeCoefficient * item.LikeNumber
            + _disLikeCoefficient * item.DisLikeNumber
            + _commentCoefficient * item.CommentNumber, 3);
    }

    private bool GetArticleByDate(Article item)
    {
        var getDate = ConvertDateTime(item.PubDate);
        if (getDate.Date == DateTime.Now.Date)
            return true;
        return false;
    }

    private DateTime ConvertDateTime(string pubDate)
    {
        string format = "ddd, dd MMM yyyy HH:mm:ss";
        try
        {
            string dateTimePart = pubDate.Substring(0, pubDate.IndexOf("GMT") - 1);

            if (DateTime.TryParseExact(dateTimePart, format, System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None, out DateTime result))
            { 
                return result;
            }
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
       return DateTime.Now;
    }

    public List<Channel> GetChannels(List<Article> data)
    {
        var channels = new List<Channel>();
        foreach (var item in data)
        {
            channels.Add(
                new Channel(item.Channel)
            );
        }

        channels = channels.GroupBy(x => x.Id).Select(group => group.First()).ToList();

        return channels;
    }

    public FollowChannel AddFavoriteChannel(string id, Channel? firstChannel, List<Channel> channels)
    {
        var channel = new FollowChannel();

        if (channels.FirstOrDefault(firstChannel) == null)
            return channel;
        channel = new FollowChannel("1", id, firstChannel.Id);
        return channel;
    }

    public BlockedChannel AddBlockedChannel(string id, Channel? firstChannel, List<Channel> channels)
    {
        var channel = new BlockedChannel();

        if (channels.FirstOrDefault(firstChannel) == null)
            return channel;
        channel = new BlockedChannel("1", id, firstChannel.Id);
        return channel;
    }

    public List<Like> AddLikeArticle(List<Like> likes, List<DisLike> disLikes, string userId, string articleId)
    {
        var item = disLikes.Where(x => x.UserId.Equals(userId) && x.ArticleId.Equals(articleId)).FirstOrDefault();
        if (item != null)
        {
            disLikes = disLikes.Where(x => !x.UserId.Equals(userId) && !x.ArticleId.Equals(articleId)).ToList();
        }
        var itemLike = new Like()
        {
            Id = "1",
            UserId = userId,
            ArticleId = articleId
        };

        likes.Add(itemLike);
        return likes;
    }

    public List<DisLike> AddDisLikeArticle(List<DisLike> disLikes, List<Like> likes, string userId, string articleId)
    {
        var item = likes.Where(x => x.UserId.Equals(userId) && x.ArticleId.Equals(articleId)).FirstOrDefault();
        if (item != null)
        {
            likes = likes.Where(x => !x.UserId.Equals(userId) && !x.ArticleId.Equals(articleId)).ToList();
        }
        var itemDisLike = new DisLike()
        {
            Id = "1",
            UserId = userId,
            ArticleId = articleId
        };

        disLikes.Add(itemDisLike);
        return disLikes;
    }

    public List<AdArticle> GetAdArticles(string url)
    {
        var adArticles = new List<AdArticle>();

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    adArticles = JsonConvert.DeserializeObject<List<AdArticle>>(jsonResponse);
                }
                else
                {
                    Console.WriteLine($"Lỗi: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        return adArticles;
    }
}