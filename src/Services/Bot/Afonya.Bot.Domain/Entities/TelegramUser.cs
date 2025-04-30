namespace Afonya.Bot.Domain.Entities;

public class TelegramUser : BaseEntity
{
    protected TelegramUser() { }

    public TelegramUser(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; private set; }
    public string Password { get; private set; }

    public void SetPassword(string password){
        this.Password = password;
    }
}
