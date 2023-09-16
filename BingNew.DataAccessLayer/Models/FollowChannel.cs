namespace BingNew.DataAccessLayer.Models
{
    public class FollowChannel
    {
        private string Id;
        private string UserId;
        private string ChannelId;   

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