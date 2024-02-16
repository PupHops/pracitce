using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace practic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
		public string? Patronimic { get; set; }
        public Course? Course { get; set; }
		public string Email { get; set; }
        public Country Country { get; set; }
		public string Password { get; set; }
		public string Phone { get; set; }
		public string? Photo { get; set; }
        public char Gender { get; set; }
        public DateTime Birthday { get; set; }
        public Role Role { get; set; }

		public string Source => Photo != null? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Users", Photo):"";
	}
}
