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
using Microsoft.EntityFrameworkCore;
using practic.ViewModels;

namespace practic.Views
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        Context db;
        User User { get; set; }
        public UserWindow(User user)
        {
            User = user;
            InitializeComponent();
            db = new Context();
            DataContext = new MainWindowViewModel();
            GetData();
        }
        private void GetData()
        {
            if (User.Role.Id == 4)
            {
                this.Title = "Окно жюри";
                var ajs = db.ActivityJuries
                    .Include(aj => aj.Activity)
                    .Include(aj => aj.Jurie)
                    .Where(aj => aj.Jurie == User)
                    .Select(aj => aj.Activity).ToList();
                List<Event> events = new List<Event>();
                foreach (var a in ajs)
                {
                    events.Add(db.ActivityEvents
                        .Include(ae => ae.Event)
                        .Include(ae => ae.Activity)
                        .Where(ae => ae.Activity == a)
                        .Select(ae => ae.Event).FirstOrDefault());
                }

                ListCocks.ItemsSource = null;
                foreach (var ev in events)
                {
                    ListCocks.Items.Add(db.Events.Include(e => e.ActivityEvents).Include(e => e.Winner).Include(e => e.City).Where(e => e.Id == ev.Id).ToList());
                }
                return;
            }
            if (User.Role.Id == 1)
            {
                ListCocks.ItemsSource = db.Events
                    .Include(e => e.City)
                    .Include(e => e.Winner)
                    .Include(e => e.ActivityEvents)
                    .Where(e=>e.Winner.Id == User.Id).ToList();
                this.Title = "Окно участника";
                return;
			}
            if (User.Role.Id == 2)
            {
                var activities = db.Activities.Include(a => a.Moderator).Where(a => a.Moderator.Id == User.Id).ToList();
                List<Event> events = new List<Event>();
                foreach (var activity in activities)
                {
					events.Add(db.ActivityEvents
	                    .Include(ae => ae.Event)
                        .ThenInclude(e=>e.Winner)
						.Include(ae => ae.Event)
						.ThenInclude(e => e.City)
						.Include(ae => ae.Activity)
	                    .Where(ae => ae.Activity == activity)
	                    .Select(ae => ae.Event).FirstOrDefault());
				}
                ListCocks.ItemsSource = events;
                this.Title = "Окно модератора";
			}
        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            Profile win = new Profile(User);
            win.Show();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Hide();
        }
    }
}
