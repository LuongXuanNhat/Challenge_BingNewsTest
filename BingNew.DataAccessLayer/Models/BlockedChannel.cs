namespace BingNew.DataAccessLayer.Models
{
    public class BlockedChannel
    {
        private string Id;
        private string UserId;
        private string ChannelId;

        public BlockedChannel()
        {
            Id = Guid.NewGuid().ToString();
            UserId = string.Empty;
            ChannelId = string.Empty;
        }

        public BlockedChannel(string Id, string userId, string channelId)
        {
            this.Id = Id;
            UserId = userId;
            ChannelId = channelId;
        }

        // Phương thức get cho Id
        public string GetId()
        {
            return Id;
        }

        // Phương thức set cho Id
        public void SetId(string value)
        {
            Id = value;
        }

        // Phương thức get cho UserId
        public string GetUserId()
        {
            return UserId;
        }

        // Phương thức set cho UserId
        public void SetUserId(string value)
        {
            UserId = value;
        }

        // Phương thức get cho ChannelId
        public string GetChannelId()
        {
            return ChannelId;
        }

        // Phương thức set cho ChannelId
        public void SetChannelId(string value)
        {
            ChannelId = value;
        }
    }
}