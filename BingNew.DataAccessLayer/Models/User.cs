public class User
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public User(string id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
}