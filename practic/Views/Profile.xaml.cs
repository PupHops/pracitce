using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using practic.Models;

namespace practic.Views
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        User User { get; set; }
        public Profile(User user)
        {
            User = user;
            InitializeComponent();
            GetData();
        }

        void GetData()
        {
            Id.Text = User.Id.ToString();
            Name.Text = User.Name;
            Surname.Text = User.Surname.ToString();
            Patronimic.Text = User.Patronimic;
            Email.Text = User.Email;
            Phone.Text = User.Phone;
        }
    }
}
