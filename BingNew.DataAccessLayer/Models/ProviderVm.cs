﻿namespace BingNew.DataAccessLayer.Models
{
    public class ProviderVm
    {
        private string Id;
        private string ChannelName;
        private string ChannelIcon ;
        private string Url ;

        public ProviderVm(string channel)
        {
            Id = channel + Guid.NewGuid().ToString();
            ChannelName = channel;
            ChannelIcon = string.Empty;
            Url = string.Empty;
        }

        public ProviderVm()
        {
            Id = string.Empty;
            ChannelName = string.Empty;
            ChannelIcon = string.Empty;
            Url = string.Empty;
        }

        public string GetChannelName()
        {
            return ChannelName;
        }
        public void SetChannelName(string channelName)
        {
            ChannelName = channelName;
        }

        public string GetId()
        {
            return Id;
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public string GetChannelIcon()
        {
            return ChannelIcon;
        }

        public void SetChannelIcon(string channelIcon)
        {
            ChannelIcon = channelIcon;
        }

        public string GetUrl()
        {
            return Url;
        }

        public void SetUrl(string url)
        {
            Url = url;
        }
    }
}