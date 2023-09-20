using System.ComponentModel.DataAnnotations.Schema;

namespace BingNew.DataAccessLayer.Models
{
    public class Provider
    {
        private string Id ;
        private string ChannelName;
        private string ChannelIcon ;
        private string Url ;

        public Provider(string channel)
        {
            Id = channel + Guid.NewGuid().ToString();
            ChannelName = channel;
        }

        public Provider()
        {
        }

        public string GetChannelName()
        {
            return ChannelName;
        }
    }
}