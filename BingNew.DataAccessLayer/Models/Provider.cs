namespace BingNew.DataAccessLayer.Models
{
    public class Provider
    {
        private Guid _providerId;
        private string _providerIcon;
        private string _providerName;

        public Provider(string providerIcon, string providerName)
        {
            _providerId = Guid.NewGuid();
            _providerIcon = providerIcon;
            _providerName = providerName;
        }

        internal string GetProviderIcon()
        {
            return _providerIcon;
        }

        internal string GetProviderName()
        {
            return _providerName;
        }
    }
}