

namespace BingNew.DataAccessLayer.Entities
{
    public partial class Topic
    {

        public string Id { get; set; } = string.Empty;
        public string TopicName { get; set; } = string.Empty;

        ////public virtual ICollection<TopicFollow> TopicFollow { get; set; }
    }
}