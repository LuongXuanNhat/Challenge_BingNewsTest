﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BingNew.DataAccessLayer.Entities
{
    public partial class ChannelBlocked
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProviderId { get; set; }

        public virtual Provider Provider { get; set; }
      //  public virtual User User { get; set; }
    }
}