namespace MangoDB
{
    public class Tool(string name, float price, int stock)
    {
        public string Name { get; set; } = name;
        public float Price { get; set; } = price;
        public int Stock { get; set; } = stock;
    }
}
