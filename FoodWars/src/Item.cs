using System.Collections.Generic;
using System.Drawing;

namespace FoodWars
{
  public class Item
  {
    public enum EType { Food, Merchandise, Beverages };

    protected Image image;
    protected string name;
    protected int price;
    protected EType type;

    public Item(string name, int price)
    {
      this.name = name;
      this.price = price;
    }

    public string GetName() => name;
    public int GetPrice() => price;
    public EType GetEType() => type;
    public Image GetImage() => image;

    public void SetPrice(int price) => this.price = price;
  }

}
