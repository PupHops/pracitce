using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic.Models
{
    public class ActivityEvent
    {
        public int Id { get; set; }
        public Activity? Activity { get; set; }
        public Event? Event { get; set; }
    }
}
