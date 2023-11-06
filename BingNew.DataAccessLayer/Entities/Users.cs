namespace BingNew.DataAccessLayer.Entities
{
    public partial class Users
    {


        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        ////public virtual ICollection<ChannelBlocked> ChannelBlocked { get; set; }
        ////public virtual ICollection<ChannelFollow> ChannelFollow { get; set; }
        ////public virtual ICollection<TopicFollow> TopicFollow { get; set; }
    }
}