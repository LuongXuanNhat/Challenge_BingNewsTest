namespace BingNew.DataAccessLayer.Models
{
    public class Provider
    {
        public string Id { get; set; } = string.Empty;
        public string ChannelName { get; set; } = string.Empty;
        public string ChannelIcon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public Provider(string channel)
        {
            ChannelName = channel;
        }

        public Provider()
        {
        }

    }
}