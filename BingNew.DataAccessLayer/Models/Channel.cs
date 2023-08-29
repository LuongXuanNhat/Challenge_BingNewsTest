namespace BingNew.DataAccessLayer.Models
{
    public class Channel
    {
        public string Id { get; set; } = string.Empty;
        public string ChannelName { get; set; } = string.Empty;
        public string ChannelIcon { get; set; } = string.Empty;

        public Channel(string channel)
        {
            ChannelName = channel;
            Id = channel;
        }

        public Channel()
        {
        }
    }
}