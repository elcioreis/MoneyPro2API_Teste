namespace MoneyPro2.Models;
public class Coin
{
    public Coin()
    {
        Nickname = String.Empty;
        Symbol = String.Empty;
        Default = false;
        Virtual = false;
        Active = true;
    }
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Symbol { get; set; }
    public bool Default { get; set; }
    public bool Virtual { get; set; }
    public bool Active { get; set; }
}

