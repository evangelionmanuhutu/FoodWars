using FoodWars.Properties;
using FoodWars.src;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWars
{
  public class Foods : Item
  {
    private List<Ingredients> receipts = new List<Ingredients>();

    public Foods(string name, int price)
      : base(name, price)
    {
      type = EType.Food;
    }

    public void AddIngredients(string name, Image image)
    {
      receipts.Add(new Ingredients(name, image));
    }

    public void Display(PictureBox pic)
    {
      switch (this.name)
      {
        case "burger": image = Resources.burger; break;
        case "ice_cream": image = Resources.icecream; break;
        case "salad": image = Resources.salad; break;
      }
      pic.Image = image;
    }

    public List<Ingredients> GetReceipts() => receipts;

    public void CopyReceipts(List<Ingredients> r)
    {
      // create new (copy) of r
      receipts = new List<Ingredients>(r);
    }

    public Ingredients GetReceipts(int idx)
    {
      return receipts[idx];
    }

    public Image GetReceiptImage(string name)
    {
      foreach (var r in receipts)
        if(r.name == name)
          return r.image;
      return null;
    }

    public string GetReceiptName(Image image)
    {
      foreach (var r in receipts)
        if (r.image == image)
          return r.name;

      return null;
    }

  }
}
