using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FoodWars
{
  public partial class MenuForm : Form
  {
    enum State
    {
      Menu, InGame
    }

    private State gameState = State.Menu;
    private Dictionary<State, Image> images;

    private PictureBox pictureBox;
    private SoundManager soundManager;

    public MenuForm()
    {
      InitializeComponent();
      LoadBackgroundImages();
      soundManager = new SoundManager();
      soundManager.PlaySound("play");
    }

    private void btNewGame_Click(object sender, EventArgs e)
    {
      GameLoop game = new GameLoop();
      game.Show();
      this.Hide();
    }

    void LoadBackgroundImages()
    {
      images = new Dictionary<State, Image>
      {
        { State.Menu, Properties.Resources.background },
        { State.InGame, Properties.Resources.food_stall },
      };
    }

    private void ExitBtn_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }
  }
}
