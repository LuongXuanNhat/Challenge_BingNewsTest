namespace BingNew.DataAccessLayer.Entities
{
    public partial class Weather
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Place { get; set; }
        public string? Icon { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public DateTime PubDate { get; set; } 
        public string? Description { get; set; }
    }
}