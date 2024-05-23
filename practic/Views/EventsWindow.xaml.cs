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
using Microsoft.EntityFrameworkCore;

namespace practic.Views
{
    /// <summary>
    /// Логика взаимодействия для EventsWindow.xaml
    /// </summary>
    public partial class EventsWindow : Window
    {
        Context db;

        public EventsWindow()
        {
            db = new Context();
            InitializeComponent();
            ListCocks.ItemsSource = db.Events.Include(e=>e.Winner).Include(e => e.City).ToList();
        }

		private void NewUser_Click(object sender, RoutedEventArgs e)
		{
            NewEventWindow window = new NewEventWindow();
            window.ShowDialog();
		}
	}
}
