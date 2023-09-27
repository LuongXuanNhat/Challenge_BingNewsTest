namespace BingNew.DataAccessLayer.Models
{
    public class Topic
    {
        public Topic(string category, Provider channel)
        {
            Name = category;
            Channels = new List<Provider> { channel };
        }
        public Topic()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Channels = new List<Provider>();
        }

        private Guid Id;
        private string Name;
        private List<Provider> Channels;

        public Guid GetId()
        {
            return Id;
        }

        // Phương thức set cho Id
        public void SetId(Guid value)
        {
            Id = value;
        }

        // Phương thức get cho Name
        public string GetName()
        {
            return Name;
        }

        // Phương thức set cho Name
        public void SetName(string value)
        {
            Name = value;
        }

        // Phương thức get cho Channels
        public List<Provider> GetChannels()
        {
            return Channels;
        }

        // Phương thức set cho Channels
        public void SetChannels(List<Provider> value)
        {
            Channels = value;
        }
    }
}