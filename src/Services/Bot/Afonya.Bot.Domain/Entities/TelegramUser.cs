namespace Afonya.Bot.Domain.Entities;

public class TelegramUser : BaseEntity
{
    protected TelegramUser() { }

    public TelegramUser(string login)
    {
        Login = login;
    }

    public string Login { get; private set; }

    public void SetLogin(string login)
    {
        Login = login;
    }
}
