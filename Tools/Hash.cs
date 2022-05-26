using MoneyPro2.ViewModels.Users;
using System.Security.Cryptography;
using System.Text;

namespace MoneyPro2.Tools;
public static class Hash
{
    public static string Generator(LoginViewModel model)
    {
        var parte01 = Generator(model.Email + model.Password);
        var parte02 = Generator("MoneyPro2" + model.Password);
        return parte01 + parte02;
    }
    public static string Generator(string input)
    {
        MD5 md5Hash = MD5.Create();
        // Converter a String para array de bytes, que é como a biblioteca trabalha.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Cria-se um StringBuilder para recompôr a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop para formatar cada byte como uma String em hexadecimal
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }
}
