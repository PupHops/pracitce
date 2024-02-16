using Microsoft.EntityFrameworkCore;

using practic.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace practic.ViewModels
{
    class ParticipantsWindowViewModel : INotifyPropertyChanged
    {
        private User selectedUser;

        private IEnumerable<User> users { get; set; }

        public IEnumerable<User> Users 
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public ParticipantsWindowViewModel()
        {
            Context db = new Context();
            users = db.Users.Include(u => u.Country).Include(u => u.Course).Include(u => u.Role).Where(u=>u.Role.Id == 1).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
