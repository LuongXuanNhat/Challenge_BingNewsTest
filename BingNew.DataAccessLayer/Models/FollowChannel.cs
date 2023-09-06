namespace BingNew.DataAccessLayer.Models
{
    public class FollowChannel
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ChannelId { get; private set; } = string.Empty;

        public FollowChannel()
        {
        }

        public FollowChannel(string Id, string userId, string channelId)
        {
            this.Id = Id;
            UserId = userId;
            ChannelId = channelId;
        }
    }
}