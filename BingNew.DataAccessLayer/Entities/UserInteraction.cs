using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.DataAccessLayer.Entities
{
    public class UserInteraction
    {
        private int Id;
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public int Likes { get; set; } = 0;
        public int Dislike { get; set; } = 0;
        public int GetId()
        {
            return Id;
        }
        public void SetId(int id)
        {
            Id = id;
        }
    }
}
