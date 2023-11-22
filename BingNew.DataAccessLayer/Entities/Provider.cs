namespace BingNew.DataAccessLayer.Entities
{
    public partial class Provider
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ChannelName { get; set; } = string.Empty;
        public string ChannelIcon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

    }
}