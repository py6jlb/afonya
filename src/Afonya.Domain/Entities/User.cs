namespace Afonya.Domain.Entities;

public class User : BaseEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public User(string login, string password, bool isAdmin = false)
    {
        Login = login;
        Password = password;
        IsAdmin = isAdmin;
    }

    public string Login { get; private set; }
    public string Password { get; private set; }
    public bool IsAdmin { get; private set; }

    public void SetPassword(string password)
    {
        this.Password = password;
    }


    public void SetAdmin(bool isAdmin)
    {
        this.IsAdmin = isAdmin;
    }

    public void SetLogin(string login)
    {
        this.Login = login;
    }
}
