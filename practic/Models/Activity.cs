using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public User Moderator { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
