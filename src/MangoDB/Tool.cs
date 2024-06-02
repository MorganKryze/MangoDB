namespace MangoDB;

public class Fn
{
    public static string Hash(string text) =>
        HashFactory.Crypto.CreateSHA2_256().ComputeString(text, Encoding.UTF8).ToString();
}
