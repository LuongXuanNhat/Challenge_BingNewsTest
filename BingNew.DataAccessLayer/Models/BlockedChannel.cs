public class BlockedChannel
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string ChannelId { get; private set; }

    public BlockedChannel()
    {
    }

    public BlockedChannel(string Id, string userId, string channelId)
    {
        this.Id = Id;
        this.UserId = userId;
        this.ChannelId = channelId;
    }
}