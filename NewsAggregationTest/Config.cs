public class Config
{
    public Config()
    {
        Headers = new RequestHeaders();
    }

    public string Type { get; internal set; }
    public string Url { get; internal set; }
    public RequestHeaders Headers { get; internal set; }
    public string Key { get; internal set; }
    public string KeyWork { get; internal set; }
    public string Language { get; internal set; }
    public string Category { get; internal set; }
    public string Channel { get; internal set; }
}