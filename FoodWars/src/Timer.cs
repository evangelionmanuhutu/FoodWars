using System;

namespace FoodWars
{
  public class Timer
  {
    public int hour = 0;
    public int minute = 0;
    public int second = 0;
    private string str;

    public Timer(int seconds)
    {
      hour = (seconds / 3600) % 60;
      minute = (seconds / 60) % 60;
      second = seconds % 60;
      str = ConvertToString();
    }

    public void Update(int seconds)
    {
      hour = (seconds / 3600) % 60;
      minute = (seconds / 60) % 60;
      second = seconds % 60;
    }

    public string ConvertToString()
    {
      string h = hour.ToString();
      string m = minute.ToString();
      string s = second.ToString();
      if (h.Length < 2)
        h = $"0{hour}";

      if (m.Length < 2)
        m = $"0{minute}";

      if (s.Length < 2)
        s = $"0{second}";

      return $"{h}:{m}:{s}";
    }
  }
}
