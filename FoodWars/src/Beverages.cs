using FoodWars.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodWars
{
  class Beverages : Foods
  {
    public bool isCold;
    public string size;
    public Beverages(string name, int price) 
      : base(name, price)
    {
     

      switch (name[0])
      {
        case 'l': size = "large";  break;
        case 'm': size = "medium";  break;
        case 's': size = "small";  break;
      }

      isCold = name[1] == 'c';

      type = EType.Beverages;
    }

    public void Display(PictureBox pic)
    {
      // Change the selected item image
      switch (this.name)
      {
        case "lhcoffee": image = Resources.coffee_L_hot; break;
        case "lccoffee": image = Resources.coffee_L_cold; break;
        case "mhcoffee": image = Resources.coffee_M_hot; break;
        case "mccoffee": image = Resources.coffee_M_cold; break;
        case "shcoffee": image = Resources.coffee_S_hot; break;
        case "sccoffee": image = Resources.coffee_S_cold; break;
      }
      pic.Image = image;
    }

  }
}
