using FoodWars.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FoodWars
{
  public class Customers
  {
    private string name;
    private string type;
    private Image image;

    public List<Item> itemList = new List<Item>();

    private int foodIndex = -1;

    public Customers(string name, string type)
    {
      this.name = name;
      this.type = type;

      switch (this.type)
      {
        case "man": image = Resources.david; break;
        case "woman": image = Resources.anna; break;
        case "kid": image = Resources.bryan; break;
      }
    }

    public Item CurrentItem()
    {
      if (itemList.Count() < 1)
        foodIndex = -1;
      else
        foodIndex = itemList.Count() - 1;

      if (foodIndex >= 0)
        return itemList[foodIndex];

      return null;
    }

    public string GetName() => name;
    public Image GetImage() => image;
  }
}
