namespace BingNew.DataAccessLayer.Models
{
    public class BlockedChannel
    {
        private string Id;
        private string UserId;
        private string ChannelId;

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