using System.Collections.Generic;

namespace FoodWars
{
  public class Item
  {
    public enum Type
    {
      Salad, Burger, IceCream,
      Plushie, Tumblr,
      // large, medium, small hot/cold coffee
      LHCoff, LCCoff, MHCoff, MCCoff, SHCoff, SCCoff,
      Drinks = LHCoff | LCCoff | MHCoff | MCCoff | SHCoff | SCCoff
    };

    public List<string> receipt = new List<string>();
    public Type type;
    public int price = 0;

    // Default constructor
    public Item() { }

    // Type and Price Construtor 
    public Item(Type type, int price)
    {
      this.type = type;
      this.price = price;
    }

    // Static method to create food type and receipts
    public static Item CreateSalad()
    {
      Item item = new Item(Type.Salad, 12000);
      item.receipt = new List<string>
      {
        "plate",
        "lettuce",
        "mayo"
      };
      return item;
    }

    public static Item CreateBurger()
    {
      Item item = new Item();
      item.receipt = new List<string>
      {
        "plate",
        "bottom_patty",
        "meat",
        "top_patty"
      };
      item.type = Type.Burger;
      item.price = 15000;

      return item;
    }

    public static Item CreateIceCream()
    {
      Item item = new Item(Type.IceCream, 15000);
      item.receipt = new List<string>
      {
        "cone",
        "ice_cream"
      };
      return item;
    }
  }

}
