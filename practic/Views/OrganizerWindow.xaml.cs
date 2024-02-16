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
    /// Логика взаимодействия для OrganizerWindow.xaml
    /// </summary>
    public partial class OrganizerWindow : Window
    {
        Context db;
        User User { get; set; }
        public OrganizerWindow(User user)
        {
            User = user;
            db = new Context();
            InitializeComponent();
            GetHelloText();
            HelloText.Text += $"\n{user.Name} {user.Patronimic}";
            Pfp.Source = new BitmapImage(new Uri(user.Source));
        }
        void GetHelloText()
        {
            if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 11)
            {
				HelloText.Text = "Доброе утро!";
				return;
            }
            if (DateTime.Now.Hour > 11 && DateTime.Now.Hour <= 18)
            {
				HelloText.Text = "Добрый день!";
				return;
            }
            if (DateTime.Now.Hour > 18 && DateTime.Now.Hour <= 0)
            {
				HelloText.Text = "Добрый вечер!";
				return;
            }
            HelloText.Text = "Доброй ночи!";
        }

		private void ProfileButton_Click(object sender, RoutedEventArgs e)
		{
            Profile win = new Profile(User);
            win.Show();
		}

		private void NewUserButton_Click(object sender, RoutedEventArgs e)
		{
            NewUser win = new NewUser();
            win.Show();
		}

        private void PartsButton_Click(object sender, RoutedEventArgs e)
        {
            ParticipantsWindow window = new ParticipantsWindow();
            window.ShowDialog();
        }
    }
}
