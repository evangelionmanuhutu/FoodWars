using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;

namespace FoodWars
{
  class SoundManager
  {
    private SoundPlayer soundPlayer;
    private Dictionary<string, string> soundFiles;

    public SoundManager()
    {
      LoadSounds();
    }

    private void LoadSounds()
    {
      try
      {
        soundFiles = new Dictionary<string, string>
        {
            { "play", @"sounds\sound_play.wav" },
            { "stop", @"sounds\sound_stop.wav" },
            { "win",  @"sounds\sound_win.wav"  },
            { "lose", @"sounds\sound_lose.wav" }
        };
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error loading sound files");
      }
    }

    public void PlaySound(string soundKey)
    {
      try
      {
        if (soundPlayer != null)
        {
          soundPlayer.Stop();
        }

        if (soundFiles.ContainsKey(soundKey))
        {
          soundPlayer = new SoundPlayer(soundFiles[soundKey]);
          soundPlayer.Play();
        }
        else
        {
          MessageBox.Show($"Sound key '{soundKey}' not found", "Error playing sound");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error playing sound");
      }
    }
  }
}
