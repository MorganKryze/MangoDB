namespace MangoDB
{
    public class Ingredient(
        string name,
        float price,
        int calories,
        int stock,
        string allergen,
        string origin
    )
    {
        public string Name { get; set; } = name;
        public float Price { get; set; } = price;
        public int Calories { get; set; } = calories;
        public int Stock { get; set; } = stock;
        public string Allergen { get; set; } = allergen;
        public string Origin { get; set; } = origin;
    }
}
