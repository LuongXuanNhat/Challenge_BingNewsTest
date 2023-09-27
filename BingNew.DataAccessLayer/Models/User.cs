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

    public string GetId()
    {
        return Id;
    }

    // Phương thức set cho Id
    public void SetId(string value)
    {
        Id = value;
    }

    // Phương thức get cho UserName
    public string GetUserName()
    {
        return UserName;
    }

    // Phương thức set cho UserName
    public void SetUserName(string value)
    {
        UserName = value;
    }

    // Phương thức get cho Email
    public string GetEmail()
    {
        return Email;
    }

    // Phương thức set cho Email
    public void SetEmail(string value)
    {
        Email = value;
    }
}