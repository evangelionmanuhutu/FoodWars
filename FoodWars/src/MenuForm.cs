using System;
using FoodWars.Properties;
using System.Linq;
using System.Windows.Forms;

namespace FoodWars
{
  public partial class MenuForm : System.Windows.Forms.Form
  {
    public GameLoop gameLoop;
    public static MenuForm instance;
    private Player player;
    private string initPlayerName;

    public MenuForm()
    {
      instance = this;
      instance.InitializeComponent();
      initPlayerName = PlayerName.Text;
    }

    private void btNewGame_Click(object sender, EventArgs e)
    {
      // we can play if the player already created
      if(player != null)
      {
        gameLoop = new GameLoop(player);
        gameLoop.Show();
        instance.Hide();
      }
      else
      {
        MessageBox.Show("Failed to create player", "Invalid name");
      }
    }

    private void ExitBtn_Click(object sender, EventArgs e)
    {
      System.Windows.Forms.Application.Exit();
    }

    private void PlayerName_Leave(object sender, EventArgs e)
    {
      if (PlayerName.Text != initPlayerName)
        player = new Player(PlayerName.Text, Resources.player);
    }
  }
}
