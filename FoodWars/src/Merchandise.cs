using FoodWars.Properties;
using System.Windows.Forms;

namespace FoodWars.src
{
  public class Merchandise : Foods
  {
    public int stock = 0;

    public Merchandise(string name, int price, int stock)
      : base(name, price)
    {
      type = Foods.EType.Merchandise;
      this.stock = stock;
    }

    public void Display(PictureBox pic)
    {
      switch (this.name)
      {
        case "tumblr": image = Resources.tumbler; break;
        case "plushie": image = Resources.plushie; break;
      }

      pic.Image = image;
    }
  }
}
