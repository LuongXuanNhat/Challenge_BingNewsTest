
using BingNew.DataAccessLayer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json.Nodes;

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
        var getDate = ConvertDateTime(item.PubDate.ToString());
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

    public List<FollowChannel> AddFavoriteChannel(string id, Channel firstChannel)
    {
        var channel = new List<FollowChannel>();
        channel.Add(new FollowChannel("1", id, firstChannel.Id));
        return channel;
    }

    public List<BlockedChannel> AddBlockedChannel(string id, Channel firstChannel)
    {
        var result = new List<BlockedChannel>();
        result.Add(new BlockedChannel("1", id, firstChannel.Id));
        return result;
    }

    public List<Like> AddLikeArticle(List<Like> likes, List<DisLike> disLikes, string userId, Guid articleId)
    {
        var item = disLikes.Where(x => x.UserId.Equals(userId) && x.ArticleId.Equals(articleId)).FirstOrDefault();
        disLikes = disLikes.Where(x => !x.UserId.Equals(userId) && !x.ArticleId.Equals(articleId)).ToList();
        var itemLike = new Like()
        {
            Id = "1",
            UserId = userId,
            ArticleId = articleId
        };

        likes.Add(itemLike);
        return likes;
    }

    public List<DisLike> AddDisLikeArticle(List<DisLike> disLikes, List<Like> likes, string userId, Guid articleId)
    {
        var item = likes.FirstOrDefault(x => x.UserId.Equals(userId) && x.ArticleId.Equals(articleId));
        likes = likes.Where(x => !x.UserId.Equals(userId) && !x.ArticleId.Equals(articleId)).ToList();

        var itemDisLike = new DisLike()
        {
            Id = "1",
            UserId = userId,
            ArticleId = articleId
        };

        disLikes.Add(itemDisLike);
        return disLikes;
    }

    public List<AdArticle>? GetAdArticles(string url)
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
                    Console.WriteLine($" errror: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" error: {ex.Message}");
            }
        }

        return adArticles;
    }

    public Comment AddComment(string userId, Guid articleId, string content)
    {
        var comment = new Comment(userId, articleId, content);
        

        return comment;
    }

    public List<Like> RemoveLikeArticle(List<Like> likes, Guid ArticleId)
    {
        var result = likes.FirstOrDefault(l => l.ArticleId.Equals(ArticleId)) ?? throw new InvalidOperationException();
            likes = likes.Where(x=> x != result).ToList();

        return likes;
    }

    public List<FollowChannel> RemoveFavoriteChannel(string userId, Channel firstChannel, List<FollowChannel> channels)
    {
        var channel = channels.First(c => c.ChannelId.Equals(firstChannel.Id) && c.UserId.Equals(userId));
        if (channel != null)
            channels = channels.Where(x=> x != channel).ToList();
        return channels;     
    }

    public List<Topic> GetTopicOfNewsChannel(List<Article> data)
    {
        var topics = new List<Topic>();
        foreach (var item in data)
        {
            var topic = CheckDuplicates(topics, item);
            if (topic != null)
            {
                topics.Add(topic);
            } else
            {
                var topic1 = topics.Where(x => x.Name.Equals(item.Category)).FirstOrDefault();
                if (topic1 != null && topic1.Channels.Where(x => x.ChannelName.Equals(item.Channel)).FirstOrDefault() == null)
                {
                    topics = topics.Where(x => x != topic1).ToList();
                    topic1.Channels.Add(new Channel(item.Channel));
                    topics.Add(topic1);
                }
            }
            
        }

        return topics;
    }

    private Topic? CheckDuplicates(List<Topic> topics, Article item)
    {
        item.Category = item.Category.Trim('[', ' ', '\r', '\n', '"', ']');
        var topic = topics.Where(x=>x.Name.Equals(item.Category)).FirstOrDefault();

        var newChannel = new Channel(item.Channel);
        topic = new Topic(item.Category, newChannel);

        return topic;
    }

    public Weather GetWeatherInfor(Config config)
    {
       
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(config.Url),
            Headers =
            {
                { "X-RapidAPI-Key", config.Headers.RapidApiKey },
                { "X-RapidAPI-Host", config.Headers.RapidApiHost },
            }
        };
        var json = JObject.Parse(GetDataWeather(request));
        return ConvertWeatherData(json, config);

    }

    private Weather ConvertWeatherData(JObject json, Config config)
    {
        var weather = new Weather();
       // var mapped = config.MappingTable;
        //var dictionary = new Dictionary<JObject, List<WeatherInfo>>();

        //if (json.TryGetValue(config.Location, out var locationToken) && locationToken.Type == JTokenType.Object)
        //{
        //    foreach (var item in mapped)
        //    {
        //        var sourceValue = locationToken[item.SourceProperty]?.ToString();
        //        var property = typeof(Weather).GetProperty(item.DestinationProperty) ?? throw new NullReferenceException();
        //            property.SetValue(weather, sourceValue);
        //    }
        //}


        return weather;
    }

    private string GetDataWeather(HttpRequestMessage request)
    {
        var client = new HttpClient();
        using (var response = client.SendAsync(request).Result)
        {
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }

    public List<MappingTable> CreateMapping(string jsonConfigMapping)
    {
        return JsonConvert.DeserializeObject<List<MappingTable>>(jsonConfigMapping) ?? new List<MappingTable>();
    }
}