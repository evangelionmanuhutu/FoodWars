using System;
using System.Windows.Forms;

namespace FoodWars
{
  public partial class MenuForm : Form
  {
    public GameLoop gameLoop;
    public static MenuForm instance;

    public MenuForm()
    {
      instance = this;
      instance.InitializeComponent();
    }

    private void btNewGame_Click(object sender, EventArgs e)
    {
      gameLoop = new GameLoop();
      gameLoop.Show();
      instance.Hide();
    }

    private void ExitBtn_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }
  }
}
