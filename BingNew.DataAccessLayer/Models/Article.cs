﻿using System.Text.Json.Serialization;

namespace BingNew.DataAccessLayer.Models
{

    public class Article : BasePost
    {
        public int LikeNumber { get; set; }
        public int DisLikeNumber { get; set; }
        public int CommentNumber { get; set; }
        public string ImageLink { get; set; }





       


    }
}