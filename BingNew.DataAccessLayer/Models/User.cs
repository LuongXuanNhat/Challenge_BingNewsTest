namespace BingNew.DataAccessLayer.Models;

public class User
{
    private string Id;
    private string UserName;
    private string Email;
    public User(string id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
}