namespace BingNew.DataAccessLayer.Models
{
    public class AdArtile : BasePost
    {

        public AdArtile(string id, string title, DateTimeOffset date, string link, string description) : base(id, title, date, link, description)
        {

        }

    }
}