namespace MangoDB;

public class Recipe(
    string name,
    float price,
    List<IngredientQuantity> ingredients,
    List<string> tools,
    List<string> steps,
    List<string> allergens,
    int calories
    )
{
    public string Name { get; set; } = name;
    public float Price { get; set; } = price;
    public List<IngredientQuantity> Ingredients { get; set; } = ingredients;
    public List<string> Tools { get; set; } = tools;
    public List<string> Steps { get; set; } = steps;
    public List<string> Allergens { get; set; } = allergens;
    public int Calories { get; set; } = calories;
}
