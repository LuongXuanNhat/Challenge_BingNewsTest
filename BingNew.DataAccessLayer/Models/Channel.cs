namespace BingNew.DataAccessLayer.Models
{
    public class Channel
    {
        public string? Id { get; set; }
        public string? ChannelName { get; set; }
        public string? ChannelIcon { get; set; }

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