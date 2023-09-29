namespace BingNew.BusinessLogicLayer.ModelConfig
{
    public class Config
    {
        public Config()
        {
            Headers = new RequestHeaders();
        }

        public string Url { get; set; } = string.Empty;
        public RequestHeaders Headers { get; set; }
        public string Key { get; set; } = string.Empty;
        public string KeyWork { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
        public string DayNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;

    }
}