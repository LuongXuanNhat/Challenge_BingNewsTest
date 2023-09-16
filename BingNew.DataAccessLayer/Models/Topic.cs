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

        private Guid Id;
        private string Name;
        private List<Provider> Channels;
    }
}