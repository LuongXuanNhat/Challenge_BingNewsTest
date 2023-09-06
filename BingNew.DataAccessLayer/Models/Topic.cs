namespace BingNew.DataAccessLayer.Models
{
    public class Topic
    {
        public Topic()
        {
            Channels = new List<Channel>();
        }

        public Topic(string category, Channel channel)
        {
            Name = category;
            Channels = new List<Channel> { channel };
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Channel> Channels { get; set; }
    }
}