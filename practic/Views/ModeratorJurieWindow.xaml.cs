using practic.Models;
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

namespace practic.Views
{
	/// <summary>
	/// Логика взаимодействия для ModeratorJurieWindow.xaml
	/// </summary>
	public partial class ModeratorJurieWindow : Window
	{
		Context db;
		private User User { get; set; }
		public ModeratorJurieWindow(User user)
		{
			db = new Context();
			User = user;
			InitializeComponent();
		}

		private void GetData()
		{
			var activities = db.Activities;
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
