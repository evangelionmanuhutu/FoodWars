using System.Drawing;

namespace FoodWars.src
{
  public class Ingredients
  {
    public string name;
    public Image image;

    public Ingredients(string name, Image image)
    {
      this.name = name;
      this.image = image;
    }
  }
}
