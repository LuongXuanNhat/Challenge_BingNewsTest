using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.DataAccessLayer.Entities
{
    public class UserClickEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
