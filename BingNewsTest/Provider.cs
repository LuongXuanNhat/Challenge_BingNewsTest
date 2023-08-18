namespace BingNewsTest
{
    public class Provider
    {
        private Guid _providerId;
        private string _providerIcon;
        private string _providerName;

        public Provider(string providerIcon, string providerName)
        {
            this._providerId = Guid.NewGuid();
            this._providerIcon = providerIcon;
            this._providerName = providerName;
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