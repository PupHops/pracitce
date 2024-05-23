using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic.Models
{
    public class Event
    {
        public static List<Event> ItemsSource { get; internal set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Days { get; set; }
        public City City { get; set; }
        public User? Winner { get; set; }
        public List<ActivityEvent> ActivityEvents { get; set; }

        public string Source => GetSource();
        public string CityName => City.Name+" ";
        public string DateOnly => Date.ToShortDateString();
        public string DaysString => Days.ToString();
        public string WinnerString => Winner == null ? "" : $"{Winner.Surname} {Winner.Name}";

        public override string ToString()
        {
            return $"{Name}";
        }

        private string GetSource()
        {
            List<String> extensions = new List<string>
            {
                ".png", ".jpg", ".jpeg"
            };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Events", Id.ToString());

			foreach (string ext in extensions)
            {
                if (File.Exists(path + ext))
                {
                    return path + ext;
                }
            }
            return null;
        }
    }
}
