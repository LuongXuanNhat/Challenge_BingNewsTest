namespace BingNew.DataAccessLayer.Entities
{
    public partial class Provider
    {
        ////public ProviderVm()
        ////{
        ////    AdArticle = new HashSet<AdArticle>();
        ////    ChannelBlocked = new HashSet<ChannelBlocked>();
        ////    ChannelFollow = new HashSet<ChannelFollow>();
        ////}

        public string Id { get; set; } = string.Empty;
        public string ChannelName { get; set; } = string.Empty;
        public string ChannelIcon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public virtual ICollection<AdArticle>? AdArticle { get; set; }
        public virtual ICollection<ChannelBlocked>? ChannelBlocked { get; set; }
        public virtual ICollection<ChannelFollow>? ChannelFollow { get; set; }
    }
}