﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BingNew.DataAccessLayer.Entities
{
    public partial class AdArticle
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string MediaLink { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }
        public string ProviderId { get; set; }

        public virtual Provider Provider { get; set; }
    }
}