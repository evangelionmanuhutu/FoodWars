using FoodWars.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace FoodWars
{
  public partial class GameLoop : Form
  {
    private List<Customer> customers;
    private List<Item> itemList;
    private List<Item> plushie;
    private List<Item> tumblr;
    private Random random = new Random();

    private int customerIndex = -1;
    private int remainingTime = 70;
    private int dialogTime = 2;
    private int coinTime = 1;
    private bool isGameRunning = true;
    private bool win = false;
    private bool dialogShown = false;
    private bool coinShown = false;
    private string playerNameVal = "Ellysa";
    private Dictionary<string, string> soundFiles;
    private SoundPlayer sfxPlayer;

    Timer timer;

    public GameLoop()
    {
      InitializeComponent();
      InitSounds();

      PlayBackgroundMusic("play");

      InitItems();

      customers = new List<Customer>
      {
        new Customer("Bryan", Customer.Type.Kid),
        new Customer("David", Customer.Type.Man),
        new Customer("Anna", Customer.Type.Woman),
        new Customer("David", Customer.Type.Man),
        new Customer("Anna", Customer.Type.Woman),
        new Customer("Bryan", Customer.Type.Kid)
      };

      PlayerName.Text = playerNameVal;

      foreach (Customer c in customers)
      {
        foreach(Item f in itemList)
        {
          if (random.Next(1, 100) % 2 == 0)
            continue;

          Item food = new Item(f.type, f.price);
          foreach (string r in f.receipt)
            food.receipt.Add(r);
          c.itemList.Add(food);
        }
          
      }

      customerIndex = random.Next(0, customers.Count() - 1);

      ShowCustomerDialog();

      timer = new Timer();
      timer.Interval = 1000;
      timer.Tick += Timer_Tick;
      timer.Start();

      SelectedImage.Image = null;
      UpdateCustomer(CurrentCustomer());
    }

    private void ShowCoin()
    {
      if(!coinShown)
      {
        CustomerDialog.Visible = false;
        CustomerTargetImage.Image = Resources.money;
        coinShown = true;
        coinTime = 1; // Reset wait time
      }
    }

    private void HideCoin()
    {
      CustomerDialog.Visible = true;
      coinShown = false;
    }

    private void ShowCustomerDialog()
    {
      ShowCoin();

      if (!dialogShown)
      {
        CustomerDialog.Visible = true;
        CustomerTargetImage.Visible = false;

        var cust = CurrentCustomer();
        CustomerDialog.Text = $"Hello I'm {cust.name}";
        dialogShown = true;
        dialogTime = 2; // Reset wait time
      }
    }

    private void HideCustomerDialog()
    {
      CustomerDialog.Visible = false;
      CustomerTargetImage.Visible = true;
      dialogShown = false;
    }

    private void InitSounds()
    {
      soundFiles = new Dictionary<string, string>
        {
            { "play", @"sounds\sound_play.wav" },
            { "stop", @"sounds\sound_stop.wav" },
            { "win",  @"sounds\sound_win.wav"  },
            { "lose", @"sounds\sound_lose.wav" },
            { "fail", @"sounds\sound_fail.wav" },
            { "correct", @"sounds\sound_correct.wav" }
        };

      // this is windows media player GUI
      MXP.settings.playCount = 10;
      MXP.Ctlcontrols.stop();

      // make it invisible (we don't need the GUI, just the sound player)
      MXP.Visible = false;
    }

    private void PlaySfx(string key)
    {
      sfxPlayer = new SoundPlayer(soundFiles[key]);
      sfxPlayer.Play();
    }

    private void PlayBackgroundMusic(string key)
    {
      MXP.URL = soundFiles[key];
      MXP.Ctlcontrols.play();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      if (isGameRunning)
      {
        remainingTime--;

        if (coinShown)
        {
          coinTime--;
          if (coinTime <= 0)
            HideCoin();
        }

        if (dialogShown)
        {
          if(!coinShown)
          {
            dialogTime--;
            if (dialogTime <= 0)
              HideCustomerDialog();
          }
        }

        UpdateTimerDisplay();

        if (remainingTime <= 0)
        {
          if (win)
          {
            this.Hide();
            MenuForm.instance.Show();
            MXP.Ctlcontrols.stop();
          }
          else
          {
            EndGame();
          }
        }
      }
    }

    private void UpdateTimerDisplay()
    {
      TimerCounter.Text = $"Time remaining: {remainingTime} s";
    }

    private void EndGame()
    {
      PlayBackgroundMusic("lose");

      isGameRunning = false;
      timer.Stop();
      MessageBox.Show("Time's up! Game Over!");
      Application.Exit();
    }

    private void IsDone(Customer customer, Item item)
    {
      if (item.receipt.Count() == 0)
      {
        PrevIncome.Text = $"Rp.{item.price}";
        customer.itemList.Remove(item);
      }

      // Remove item from list if it is
      // already picked up
      if(customer.itemList.Count() == 0)
      {
        var name = customer.name;

        customers.Remove(customer);


        // Check if we have any customers
        if (customers.Count > 0)
        {
          // choose random customer
          customerIndex = random.Next(0, customers.Count() - 1);

          int repeated = 0;
          // keep randoming if we get the same customer
          while (CurrentCustomer().name == name)
          {
            // but we only repeated twice if we still
            // get the same customer
            if (repeated > 2)
              break;

            customerIndex = random.Next(0, customers.Count() - 1);
            repeated++;
          }
        }
        
        PlaySfx("correct");

        ShowCustomerDialog();

        PrevTime.Text = $"{remainingTime}";
      }

      win = customers.Count() == 0;
      if (win)
      {
        PlayBackgroundMusic("win");
        remainingTime = 6;
      }

      UpdateCustomer(CurrentCustomer());
    }

    private void InitItems()
    {
      itemList = new List<Item>
      {
        Item.CreateBurger(),
        Item.CreateSalad(),
        Item.CreateIceCream()
      };

      itemList.Add(new Item(Item.Type.LHCoff, 25000));
      itemList.Add(new Item(Item.Type.LCCoff, 27000));
      itemList.Add(new Item(Item.Type.MHCoff, 20000));
      itemList.Add(new Item(Item.Type.MCCoff, 22000));
      itemList.Add(new Item(Item.Type.SHCoff, 15000));
      itemList.Add(new Item(Item.Type.SCCoff, 17000));
      itemList.Add(new Item(Item.Type.Plushie, 20000));
      itemList.Add(new Item(Item.Type.Tumblr, 25000));

      plushie = new List<Item>
      {
        new Item(Item.Type.Plushie, 20000),
        new Item(Item.Type.Plushie, 20000),
        new Item(Item.Type.Plushie, 20000),
        new Item(Item.Type.Plushie, 20000),
        new Item(Item.Type.Plushie, 20000),
        new Item(Item.Type.Plushie, 20000)
      };

      tumblr = new List<Item>
      {
        new Item(Item.Type.Tumblr, 25000),
        new Item(Item.Type.Tumblr, 25000),
        new Item(Item.Type.Tumblr, 25000),
        new Item(Item.Type.Tumblr, 25000),
        new Item(Item.Type.Tumblr, 25000),
        new Item(Item.Type.Tumblr, 25000)
      };

      TumblrCounter.Text = $"{tumblr.Count()}x";
      PlushieCounter.Text = $"{plushie.Count()}x";
    }

    private void WrongItmeSelected()
    {
      PlaySfx("fail");
      SelectedImage.Image = Resources.wrong;
    }

    private Customer CurrentCustomer()
    {
      // this is should be a positive value
      if (customerIndex >= 0)
      {
        var c = customers[customerIndex];
        UpdateCustomer(c);

        var item = c.GetCurrentItem();
        return c;
      }

      return null;
    }

    private void UpdateCustomer(Customer customer)
    {
      // Exit this function if the customer is invalid
      if (customer == null)
        return;

      // Upadate customer dialog (item image)
      var item = customer.GetCurrentItem();
      if (item != null)
      {
        switch (item.type)
        {
          case Item.Type.Salad:
            CustomerTargetImage.Image = Resources.salad;
            break;
          case Item.Type.Burger:
            CustomerTargetImage.Image = Resources.burger;
            break;
          case Item.Type.Plushie:
            CustomerTargetImage.Image = Resources.plushie;
            break;
          case Item.Type.Tumblr:
            CustomerTargetImage.Image = Resources.tumbler;
            break;
          case Item.Type.IceCream:
            CustomerTargetImage.Image = Resources.icecream;
            break;
          case Item.Type.LHCoff:
            CustomerTargetImage.Image = Resources.coffee_L_hot;
            break;
          case Item.Type.LCCoff:
            CustomerTargetImage.Image = Resources.coffee_L_cold;
            break;
          case Item.Type.MHCoff:
            CustomerTargetImage.Image = Resources.coffee_M_hot;
            break;
          case Item.Type.MCCoff:
            CustomerTargetImage.Image = Resources.coffee_M_cold;
            break;
          case Item.Type.SHCoff:
            CustomerTargetImage.Image = Resources.coffee_S_hot;
            break;
          case Item.Type.SCCoff:
            CustomerTargetImage.Image = Resources.coffee_S_cold;
            break;
        }
      }

      // Update customer counter
      CustomerCounter.Text = customers.Count().ToString();

      switch(customer.type)
      {
        case Customer.Type.Woman:
          CustomerImage.Image = Resources.anna;
          break;
        case Customer.Type.Man:
          CustomerImage.Image = Resources.david;
          break;
        case Customer.Type.Kid:
          CustomerImage.Image = Resources.bryan;
          break;
      }
      
    }

    private void Plate_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.Drinks || item.type == Item.Type.IceCream)
        WrongItmeSelected();

      if (item.receipt.Count() > 0)
      {
        // check the very first element (at index 0)
        // this is SHOULD be plate
        if (item.receipt[0] == "plate")
        {
          // Change the selected item image
          SelectedImage.Image = Resources.plate;
          // if it found, then remove it
          item.receipt.RemoveAt(0);
          IsDone(cust, item);
        }
        else
        {
          WrongItmeSelected();
          MessageBox.Show(item.receipt[0]);
        }
      }
    }

    private void BottomPatty_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.Drinks || item.type == Item.Type.IceCream)
        WrongItmeSelected();

      if (item.receipt.Count() > 0)
      {
        // check the very first element (at index 0)
        // this is SHOULD be bottom_patty
        if (item.receipt[0] == "bottom_patty")
        {
          // Change the selected item image
          SelectedImage.Image = Resources.bottompan;
          // if it found, then remove it
          item.receipt.RemoveAt(0);

          IsDone(cust, item);
        }
        else
        {
          WrongItmeSelected();
        }
      }
    }

    private void Meat_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.Drinks || item.type == Item.Type.IceCream)
        WrongItmeSelected();

      if (item.receipt.Count() > 0)
      {
        // check the very first element (at index 0)
        // this is SHOULD be meat
        if (item.receipt[0] == "meat")
        {
          // Change the selected item image
          SelectedImage.Image = Resources.patty;
          // if it found, then remove it
          item.receipt.RemoveAt(0);

          IsDone(cust, item);
        }
        else
        {
          WrongItmeSelected();
        }
      }
    }

    private void Lettuce_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.Drinks || item.type == Item.Type.IceCream)
        WrongItmeSelected();

      if (item.receipt.Count() > 0)
      {
        // check the very first element (at index 0)
        // this is SHOULD be lettuce
        if (item.receipt[0] == "lettuce")
        {
          // Change the selected item image
          SelectedImage.Image = Resources.lettuce;
          // if it found, then remove it
          item.receipt.RemoveAt(0);

          IsDone(cust, item);
        }
        else
        {
          WrongItmeSelected();
        }
      }
    }

    private void TopPatty_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.Drinks || item.type == Item.Type.IceCream)
        WrongItmeSelected();

      if (item.receipt.Count() > 0)
      {
        // check the very first element (at index 0)
        // this is SHOULD be top_patty
        if (item.receipt[0] == "top_patty")
        {
          // Change the selected item image
          SelectedImage.Image = Resources.toppan;
          // if it found, then remove it
          item.receipt.RemoveAt(0);
          IsDone(cust, item);
        }
        else
        {
          WrongItmeSelected();
        }
      }
    }

    private void Mayo_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.Drinks || item.type == Item.Type.IceCream)
        WrongItmeSelected();

      if (item.receipt.Count() > 0)
      {
        // check the very first element (at index 0)
        // this is SHOULD be mayo
        if (item.receipt[0] == "mayo")
        {
          // Change the selected item image
          SelectedImage.Image = Resources.mayo;
          // if it found, then remove it
          item.receipt.RemoveAt(0);
          IsDone(cust, item);
        }
        else
        {
          WrongItmeSelected();
        }
      }
    }

    private void Cone_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;
        

      if(item.type == Item.Type.IceCream)
      {
        if (item.receipt.Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be cone
          if (item.receipt[0] == "cone")
          {
            // Change the selected item image
            SelectedImage.Image = Resources.cone;
            // if it found, then remove it
            item.receipt.RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItmeSelected();
          }
        }
        else
        {
          WrongItmeSelected();
        }
      }
    }

    private void Ice_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.IceCream)
      {
        if (item.receipt.Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be ice_cream
          if (item.receipt[0] == "ice_cream")
          {
            // Change the selected item image
            SelectedImage.Image = Resources.ice;
            // if it found, then remove it
            item.receipt.RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItmeSelected();
          }
        }
      }
      else
      {
        WrongItmeSelected();
      }
    }

    private void Tumblr_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type != Item.Type.Tumblr)
        WrongItmeSelected();

      if(tumblr.Count() > 0)
      {
        PrevIncome.Text = $"Rp.{item.price}";
        tumblr.RemoveAt(tumblr.Count() - 1);
        cust.itemList.Remove(item);
        TumblrCounter.Text = $"{tumblr.Count()}x";
        // Change the selected item image
        SelectedImage.Image = Resources.tumbler;
        IsDone(cust, item);
      }
      else
      {
        MessageBox.Show("Tumblr is unavailable", "Tumblr");
      }
    }

    private void Plushie_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type != Item.Type.Plushie)
        WrongItmeSelected();

      if (plushie.Count() > 0)
      {
        PrevIncome.Text = $"Rp.{item.price}";
        plushie.RemoveAt(plushie.Count() - 1);
        cust.itemList.Remove(item);
        PlushieCounter.Text = $"{plushie.Count()}x";

        // Change the selected item image
        SelectedImage.Image = Resources.plushie;
        IsDone(cust, item);
      }
      else
      {
        MessageBox.Show("Tumblr is unavailable", "Tumblr");
      }
    }

    private void CoffLCold_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.LCCoff)
      {
        // Change the selected item image
        SelectedImage.Image = Resources.coffee_L_cold;
        IsDone(cust, item);
      }
      else
      {
        WrongItmeSelected();
      }
    }

    private void CoffLHot_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.LHCoff)
      {
        // Change the selected item image
        SelectedImage.Image = Resources.coffee_L_hot;
        IsDone(cust, item);
      }
      else
      {
        WrongItmeSelected();
      }
    }

    private void CoffMCold_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.MCCoff)
      {
        // Change the selected item image
        SelectedImage.Image = Resources.coffee_M_cold;
        IsDone(cust, item);
      }
      else
      {
        WrongItmeSelected();
      }
    }

    private void CoffMHot_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.MHCoff)
      {
        // Change the selected item image
        SelectedImage.Image = Resources.coffee_M_hot;
        IsDone(cust, item);
      }
      else
      {
        WrongItmeSelected();
      }
    }

    private void CoffSCold_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.SCCoff)
      {
        // Change the selected item image
        SelectedImage.Image = Resources.coffee_S_cold;
        IsDone(cust, item);
      }
      else
      {
        WrongItmeSelected();
      }
    }

    private void CoffSHot_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.GetCurrentItem();
      if (item == null)
        return;

      if (item.type == Item.Type.SHCoff)
      {
        // Change the selected item image
        SelectedImage.Image = Resources.coffee_S_hot;
        IsDone(cust, item);
      }
      else
      {
        WrongItmeSelected();
      }
    }
  }
}
