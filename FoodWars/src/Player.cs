using System.Drawing;

namespace FoodWars
{
  public class Player
  {
    private string name;
    private Image image;
    private int income;

    public Player(string name, Image image)
    {
      this.name = name;
      this.image = image;
    }

    public string GetName() => name;
    public Image GetImage() => image;
    public int GetIncome() => income;

    public void SetIncome(int income) => this.income = income;
  }
}
