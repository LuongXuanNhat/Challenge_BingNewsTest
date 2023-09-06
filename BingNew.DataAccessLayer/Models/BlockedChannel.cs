namespace BingNew.DataAccessLayer.Models
{
    public class BlockedChannel
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ChannelId { get; private set; } = string.Empty;

        public BlockedChannel()
        {
        }

        public BlockedChannel(string Id, string userId, string channelId)
        {
            this.Id = Id;
            UserId = userId;
            ChannelId = channelId;
        }
    }
}