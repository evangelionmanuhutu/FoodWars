using FoodWars.Properties;
using FoodWars.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace FoodWars
{
  public partial class GameLoop : Form
  {
    private List<Customers> customerList;
    private List<Item> itemList;
    private Random random = new Random();

    private int customerIndex = -1;
    private int remainingTime = 100;
    private int dialogTime = 2;
    private int coinTime = 2;
    private bool isGameRunning = true;
    private bool win = false;
    private bool dialogShown = false;
    private bool coinShown = false;

    private int plushieStock = 2;
    private int tumblrStock = 5;

    private Dictionary<string, string> soundFiles;
    private SoundPlayer sfxPlayer;
    private Player myPlayer;

    private System.Windows.Forms.Timer tickTimer;
    private Timer timer;

    public GameLoop(Player player)
    {
      this.myPlayer = player;
      InitializeComponent();
      InitSounds();

      PlayBackgroundMusic("play");

      InitItems();

      customerList = new List<Customers>
      {
        new Customers("Bryan", "kid"),
        new Customers("David", "man"),
        new Customers("Anna", "woman"),
        new Customers("David", "man"),
        new Customers("Anna", "woman"),
        new Customers("Bryan", "kid")
      };

      PlayerName.Text = myPlayer.GetName();

      // loop through all of customers
      foreach (var c in customerList)
      {
        // loop through all of itemList
        foreach (var item in itemList)
        {
          // generate random number
          // add the food to customer's list
          // if it is an odd number
          var num = random.Next(1, itemList.Count());
          if (num % 2 != 0)
          {
            // Check the item's type
            if (item.GetEType() == Item.EType.Food)
            {
              // Cast to the type
              var i = item as Foods;
              Foods food = new Foods(i.GetName(), i.GetPrice());
              // copy - paste the all receipts
              // (don't cut - paste)
              // you can check the function
              food.CopyReceipts(i.GetReceipts());
              c.itemList.Add(food);
            }
            else if (item.GetEType() == Item.EType.Beverages)
            {
              // same but we don't need the receipts
              var i = item as Beverages;
              Beverages bev = new Beverages(i.GetName(), i.GetPrice());
              c.itemList.Add(bev);
            }
            else if (item.GetEType() == Item.EType.Merchandise)
            {
              // same but we don't need the receipts
              var i = item as Merchandise;
              Merchandise merch = new Merchandise(i.GetName(), i.GetPrice(), i.stock);
              c.itemList.Add(merch);
            }
          }
        }
      }

      customerIndex = random.Next(0, customerList.Count() - 1);

      ShowCustomerDialog();

      // setup Windows System Tick Timer
      tickTimer = new System.Windows.Forms.Timer();
      // this is how fast it increment
      // (each 1000 milliseconds - 1 second)
      tickTimer.Interval = 1000;
      // add Event to the timer so we can use the tick timer
      tickTimer.Tick += Timer_Tick;
      // start the tick timer
      tickTimer.Start();


      // this is our custom timer
      // only need seconds and we convert it inside
      timer = new Timer(remainingTime);

      // init timer display to current time
      UpdateTimerDisplay();

      SelectedImage.Image = null;
      UpdateCustomer(CurrentCustomer());
    }

    private void ShowCoin()
    {
      if (!coinShown)
      {
        coinShown = true;
        CustomerTargetImage.Visible = true;
        CustomerDialog.Visible = false;
        CustomerTargetImage.Image = Resources.money;
        coinTime = 2; // Reset wait time
      }
    }

    private void HideCoin()
    {
      CustomerDialog.Visible = true;
      coinShown = false;
    }

    private void ShowCustomerDialog()
    {
      if (!dialogShown)
      {
        dialogShown = true;
        CustomerDialog.Visible = true;
        CustomerTargetImage.Visible = false;

        CustomerDialog.Text = $"Hello I'm {CurrentCustomer().GetName()}";
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
      // Load all sounds from out computer
      // store by Dictionary [Key] -> Value
      // we can access the filepath by the key
      // example: var audioFile = soundFiles["play"];
      // output : audioFile -> sounds\sound_play.wav
      soundFiles = new Dictionary<string, string>
        {
            { "play", @"sounds\sound_play.wav" },
            { "stop", @"sounds\sound_stop.wav" },
            { "win",  @"sounds\sound_win.wav"  },
            { "lose", @"sounds\sound_lose.wav" },
            { "fail", @"sounds\sound_fail.wav" },
            { "pass", @"sounds\sound_pass.wav" },
            { "correct", @"sounds\sound_correct.wav" }
        };

      // this is Windows Media Player GUI
      MXP.settings.playCount = 10; // repeat 10 times if it is stop
      MXP.Ctlcontrols.stop(); // stop at initialization

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
      // URL -> filepath
      MXP.URL = soundFiles[key];
      MXP.Ctlcontrols.play();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      // this function is called once per second
      // check if the game is running
      if (isGameRunning)
      {
        // decrease remaining time if by 1 each second
        remainingTime--;
        // and then update our timer time
        timer.Update(remainingTime);

        if (coinShown)
        {
          coinTime--;
          if (coinTime <= 0)
          {
            if (!win)
            {
              var customer = CurrentCustomer();
              // store the customer's name to temporary name variable
              string name = customer.GetName();

              // then remove the customer from the list
              customerList.Remove(customer);

              // we are won if the customerList are empty
              // which means all of the customers are served
              win = customerList.Count() == 0;

              // Check if we have any customers
              if (customerList.Count > 0)
              {
                // choose random customer from 0 to the size of customerList - 1
                customerIndex = random.Next(0, customerList.Count() - 1);

                int repeated = 0;
                // keep randoming if we get the same customer
                // compare current customer's name to our temporary name variable
                while (CurrentCustomer().GetName() == name)
                {
                  // but we only repeated twice if we still
                  // get the same customer
                  if (repeated > 2)
                    break;

                  // choose random customer from 0 to the size of customerList - 1
                  customerIndex = random.Next(0, customerList.Count() - 1);
                  repeated++;
                }

                // this is the next customer's dialog
                ShowCustomerDialog();

                // set the previous time to the current timer
                PrevTime.Text = timer.ConvertToString();
              }

              if(!win)
              {
                HideCoin();
              }
            }
          }
        }
        else
        {
          if (dialogShown)
          {
            dialogTime--;
            if (dialogTime <= 0)
              HideCustomerDialog();
          }
        }

        if (win)
        {
          isGameRunning = false;

          PlayBackgroundMusic("win");

          MessageBox.Show($"{myPlayer.GetName()} You Win!!\n" +
            $"Your Income: Rp. {myPlayer.GetIncome()}\n" +
            $"You Remaining Time:  {timer.ConvertToString()}");

          Application.Exit();
        }

        // if time's up
        if (remainingTime <= 0)
          EndGame();
        else if (remainingTime > 0 && !win)
          UpdateTimerDisplay();
      }
    }

    private void UpdateTimerDisplay()
    {
      TimerCounter.Text = timer.ConvertToString();
    }

    private void EndGame()
    {
      PlayBackgroundMusic("lose");

      isGameRunning = false;
      tickTimer.Stop();
      MessageBox.Show("Time's up! Game Over!");
      Application.Exit();
    }

    private void IsDone(Customers customer, Item item)
    {
      // this is only checks if the recepits is empty
      // if true then update myPlayer's income
      if (item.GetEType() == Item.EType.Food)
      {
        var i = item as Foods;
        if (i.GetReceipts().Count() == 0)
        {
          PlaySfx("correct");

          // add item's price to myPlayerIncome
          // and store them to income
          // and set myPlayerIncome
          var income = myPlayer.GetIncome() + item.GetPrice();
          myPlayer.SetIncome(income);

          PrevIncome.Text = $"Rp.{myPlayer.GetIncome()}";

          // the receipts is already empty
          // it means the item is served to customer
          // remove it
          customer.itemList.Remove(item);
        }
        
      }
      else
      {
        PlaySfx("correct");
        var income = myPlayer.GetIncome() + item.GetPrice();
        myPlayer.SetIncome(income);

        PrevIncome.Text = $"Rp.{myPlayer.GetIncome()}";

        customer.itemList.Remove(item);
      }


      bool complete = customer.itemList.Count() == 0;

      if (complete && !coinShown)
      {
        PlaySfx("pass");
        ShowCoin();
      }

      if (!win)
        UpdateCustomer(CurrentCustomer());
    }

    private void InitItems()
    {
      /*
       * in the init items we are setups
       * all of the items (foods, beverages, and merchandises)
       * 
       * initialize the itemList first
       * 
       * and create the items one by one
       * add ingredients to the item
       * and finaly add the item to the itemList
       */
      itemList = new List<Item>();

      Foods burger = new Foods("burger", 50000);
      burger.AddIngredients("plate", Resources.plate);
      burger.AddIngredients("bottom_pan", Resources.bottompan);
      burger.AddIngredients("patty", Resources.patty);
      burger.AddIngredients("lettuce", Resources.lettuce);
      burger.AddIngredients("top_pan", Resources.toppan);
      itemList.Add(burger);

      Foods salad = new Foods("salad", 25000);
      salad.AddIngredients("plate", Resources.plate);
      salad.AddIngredients("lettuce", Resources.salad);
      salad.AddIngredients("mayo", Resources.mayo);
      itemList.Add(salad);

      Foods iceCream = new Foods("ice_cream", 10000);
      iceCream.AddIngredients("cone", Resources.cone);
      iceCream.AddIngredients("ice", Resources.ice);
      itemList.Add(iceCream);

      Beverages largeColdCoffee = new Beverages("lccoffee", 25000);
      Beverages largeHotCoffee = new Beverages("lhcoffee", 25000);
      Beverages mediumColdCoffee = new Beverages("mccoffee", 20000);
      Beverages mediumHotCoffee = new Beverages("mhcoffee", 20000);
      Beverages smallColdCoffee = new Beverages("sccoffee", 15000);
      Beverages smallHotCoffee = new Beverages("shcoffee", 15000);

      itemList.Add(largeColdCoffee);
      itemList.Add(largeHotCoffee);
      itemList.Add(mediumColdCoffee);
      itemList.Add(mediumHotCoffee);
      itemList.Add(smallColdCoffee);
      itemList.Add(smallHotCoffee);

      Merchandise tumblr = new Merchandise("tumblr", 50000, tumblrStock);
      Merchandise plushie = new Merchandise("plushie", 100000, plushieStock);
      itemList.Add(tumblr);
      itemList.Add(plushie);

      // dont forget to initialize the 
      // merchand's stock text
      TumblrCounter.Text = $"{tumblrStock}x";
      PlushieCounter.Text = $"{plushieStock}x";
    }

    private void WrongItemClicked()
    {
      PlaySfx("fail");
      SelectedImage.Image = Resources.wrong;
    }

    private Customers CurrentCustomer()
    {
      // this is should be a positive value
      if (customerIndex >= 0 && customerList.Count() > 0)
      {
        var c = customerList[customerIndex];
        UpdateCustomer(c);
        return c;
      }

      return null;
    }

    private void UpdateCustomer(Customers customer)
    {
      // Exit this function
      // if the customer or item is invalid
      var item = customer.CurrentItem();
      if (customer == null || item == null)
        return;

      // Upadate customer dialog (item image)
      switch (item.GetEType())
      {
        case Item.EType.Food:
          var food = item as Foods;
          food.Display(CustomerTargetImage);
          break;
        case Item.EType.Merchandise:
          var merch = item as Merchandise;
          merch.Display(CustomerTargetImage);
          break;
        case Item.EType.Beverages:
          var bev = item as Beverages;
          bev.Display(CustomerTargetImage);
          break;
      }

      // Update customer counter
      CustomerCounter.Text = customerList.Count().ToString();
      CustomerImage.Image = customer.GetImage();
    }

    private void Plate_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Food)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be plate
          if (item.GetReceipts(0).name == "plate")
          {
            // Change the selected item image
            SelectedImage.Image = item.GetReceipts(0).image;

            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
    }

    private void BottomPan_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Food)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be bottom_pan
          if (item.GetReceipts(0).name == "bottom_pan")
          {
            // Change the selected item image
            SelectedImage.Image = item.GetReceipts(0).image;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
    }

    private void Patty_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Food)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be patty
          if (item.GetReceipts(0).name == "patty")
          {
            // Change the selected item image
            SelectedImage.Image = item.GetReceipts(0).image;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);

            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
    }

    private void Lettuce_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Food)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be lettuce
          if (item.GetReceipts(0).name == "lettuce")
          {
            // Change the selected item image
            SelectedImage.Image = item.GetReceipts(0).image;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);

            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
    }

    private void TopPan_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Food)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be top_pan
          if (item.GetReceipts(0).name == "top_pan")
          {
            // Change the selected item image
            SelectedImage.Image = item.GetReceipts(0).image;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
    }

    private void Mayo_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() == Item.EType.Beverages || item.GetName() == "ice_cream")
      {
        WrongItemClicked();
      }
      else
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be mayo
          if (item.GetReceipts(0).name == "mayo")
          {
            // Change the selected item image
            SelectedImage.Image = Resources.mayo;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
    }

    private void Cone_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetName() == "ice_cream")
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be cone
          if (item.GetReceipts(0).name == "cone")
          {
            // Change the selected item image
            SelectedImage.Image = Resources.cone;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
      else
      {
        WrongItemClicked();
      }
    }

    private void Ice_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Foods;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetName() == "ice_cream")
      {
        if (item.GetReceipts().Count() > 0)
        {
          // check the very first element (at index 0)
          // this is SHOULD be ice
          if (item.GetReceipts(0).name == "ice")
          {
            // Change the selected item image
            SelectedImage.Image = Resources.ice;
            // if it found, then remove it
            item.GetReceipts().RemoveAt(0);
            IsDone(cust, item);
          }
          else
          {
            WrongItemClicked();
          }
        }
      }
      else
      {
        WrongItemClicked();
      }
    }

    private void Tumblr_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Merchandise;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Merchandise || item.GetName() != "tumblr")
      {
        WrongItemClicked();
      }
      else
      {
        if (tumblrStock > 0)
        {
          tumblrStock--;
          TumblrCounter.Text = $"{tumblrStock}x";

          cust.itemList.Remove(item);

          // Change the selected item image
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
        else
        {
          item.Display(SelectedImage);

          MessageBox.Show("Tumblr is unavailable", "Tumblr");
          cust.itemList.Remove(item);
          item.SetPrice(0);
          IsDone(cust, item);
        }
      }
    }

    private void Plushie_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Merchandise;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Item.EType.Merchandise || item.GetName() != "plushie")
      {
        WrongItemClicked();
      }
      else
      {
        if (plushieStock > 0)
        {
          plushieStock--;
          PlushieCounter.Text = $"{plushieStock}x";

          cust.itemList.Remove(item);

          // Change the selected item image
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
        else
        {
          item.Display(SelectedImage);

          MessageBox.Show("Plushie is unavailable", "Plushie");
          cust.itemList.Remove(item);
          item.SetPrice(0);
          IsDone(cust, item);
        }
      }
    }

    private void LColdCoffee_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Beverages;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Foods.EType.Beverages)
      {
        WrongItemClicked();
      }
      else
      {
        if (!item.isCold || item.size != "large")
        {
          WrongItemClicked();
        }
        else
        {
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
      }
    }

    private void LHotCoffee_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Beverages;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Foods.EType.Beverages)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.isCold || item.size != "large")
        {
          WrongItemClicked();
        }
        else
        {
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
      }
    }

    private void MColdCoffee_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Beverages;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Foods.EType.Beverages)
      {
        WrongItemClicked();
      }
      else
      {
        if (!item.isCold || item.size != "medium")
        {
          WrongItemClicked();
        }
        else
        {
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
      }
    }

    private void MHotCoffee_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Beverages;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Foods.EType.Beverages)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.isCold || item.size != "medium")
        {
          WrongItemClicked();
        }
        else
        {
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
      }
    }

    private void SColdCoffee_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Beverages;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Foods.EType.Beverages)
      {
        WrongItemClicked();
      }
      else
      {
        if (!item.isCold || item.size != "small")
        {
          WrongItemClicked();
        }
        else
        {
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
      }
    }

    private void SHotCoffee_Click(object sender, EventArgs e)
    {
      var cust = CurrentCustomer();
      if (cust == null)
        return;

      var item = cust.CurrentItem() as Beverages;
      if (item == null)
      {
        WrongItemClicked();
        return;
      }

      if (item.GetEType() != Foods.EType.Beverages)
      {
        WrongItemClicked();
      }
      else
      {
        if (item.isCold || item.size != "small")
        {
          WrongItemClicked();
        }
        else
        {
          item.Display(SelectedImage);
          IsDone(cust, item);
        }
      }
    }
  }
}
