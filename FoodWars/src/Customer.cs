using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWars
{
  public class Customer
  {
    public enum Type
    {
      Woman, Man, Kid
    }

    public string name;
    public Type type;
    public List<Item> itemList = new List<Item>();

    public int foodIndex = -1;

    public Customer(string name, Type type)
    {
      this.name = name;
      this.type = type;
    }

    public Item GetCurrentItem()
    {
      if (itemList.Count() < 1)
        foodIndex = -1;
      else
        foodIndex = itemList.Count() - 1;

      if (foodIndex >= 0)
        return itemList[foodIndex];

      return null;
    }
  }
}
