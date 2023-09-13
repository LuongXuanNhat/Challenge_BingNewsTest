namespace BingNew.DataAccessLayer.Models
{
    public class Topic
    {
        public Topic()
        {
            Channels = new List<Provider>();
        }

        public Topic(string category, Provider channel)
        {
            Name = category;
            Channels = new List<Provider> { channel };
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Provider> Channels { get; set; }
    }
}