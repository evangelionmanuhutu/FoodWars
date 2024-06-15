using FoodWars.Properties;

namespace FoodWars
{
  partial class MenuForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.NewGameBt = new System.Windows.Forms.Button();
      this.ExitBtn = new System.Windows.Forms.Button();
      this.background = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.background)).BeginInit();
      this.SuspendLayout();
      // 
      // NewGameBt
      // 
      this.NewGameBt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      this.NewGameBt.Font = new System.Drawing.Font("Humnst777 BlkCn BT", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewGameBt.ForeColor = System.Drawing.SystemColors.InfoText;
      this.NewGameBt.Location = new System.Drawing.Point(276, 277);
      this.NewGameBt.Name = "NewGameBt";
      this.NewGameBt.Size = new System.Drawing.Size(175, 43);
      this.NewGameBt.TabIndex = 1;
      this.NewGameBt.Text = "New Game";
      this.NewGameBt.UseVisualStyleBackColor = false;
      this.NewGameBt.Click += new System.EventHandler(this.btNewGame_Click);
      // 
      // ExitBtn
      // 
      this.ExitBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
      this.ExitBtn.Font = new System.Drawing.Font("Humnst777 BlkCn BT", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExitBtn.ForeColor = System.Drawing.SystemColors.InfoText;
      this.ExitBtn.Location = new System.Drawing.Point(276, 331);
      this.ExitBtn.Name = "ExitBtn";
      this.ExitBtn.Size = new System.Drawing.Size(175, 43);
      this.ExitBtn.TabIndex = 2;
      this.ExitBtn.Text = "Exit";
      this.ExitBtn.UseVisualStyleBackColor = false;
      this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
      // 
      // background
      // 
      this.background.BackgroundImage = global::FoodWars.Properties.Resources.background;
      this.background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.background.Image = global::FoodWars.Properties.Resources.background;
      this.background.Location = new System.Drawing.Point(0, 0);
      this.background.MaximumSize = new System.Drawing.Size(730, 450);
      this.background.MinimumSize = new System.Drawing.Size(730, 450);
      this.background.Name = "background";
      this.background.Size = new System.Drawing.Size(730, 450);
      this.background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.background.TabIndex = 0;
      this.background.TabStop = false;
      // 
      // MenuForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(730, 450);
      this.Controls.Add(this.ExitBtn);
      this.Controls.Add(this.NewGameBt);
      this.Controls.Add(this.background);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximumSize = new System.Drawing.Size(730, 450);
      this.MinimumSize = new System.Drawing.Size(730, 450);
      this.Name = "MenuForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Food Wars";
      ((System.ComponentModel.ISupportInitialize)(this.background)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox background;
    private System.Windows.Forms.Button NewGameBt;
    private System.Windows.Forms.Button ExitBtn;
  }
}

