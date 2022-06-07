namespace MoneyPro2.Models;
public class Login
{
    public Login()
    {
        User = new User();
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime LoginDate { get; set; }
    public string? IpAddress { get; set; }
    public User User { get; set; }
}

