using EasyCaptcha.Wpf;

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
using System.Windows.Threading;
using practic.Models;
using Microsoft.EntityFrameworkCore;
using practic.Views;

namespace practic
{
	/// <summary>
	/// Логика взаимодействия для Authorization.xaml
	/// </summary>
	public partial class Authorization : Window
	{
		Context db;
		String _answer;
		int _counter = 0;
		DispatcherTimer _timer;
		int sec = 0;

		public Authorization()
		{
			db = new Context();
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromSeconds(1);
			_timer.Tick += timer_Tick;
			InitializeComponent();
			CreateCaptcha();
		}
		void timer_Tick(object sender, EventArgs e)
		{
			sec++;
			if(sec == 10)
			{
				sec = 0;
				_counter = 0;
				LogInButton.IsEnabled = true;
				_timer.Stop();
			}
		}
		private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
		{
			CreateCaptcha();
		}

		private void CreateCaptcha()
		{
			Captcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 4);
			_answer = Captcha.CaptchaText;
		}
		void CheckCounter()
		{
			if (_counter == 3)
			{
				_timer.Start();
				LogInButton.IsEnabled = false;
			}
		}

		private void LogInButton_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(PasswordText.Password) || String.IsNullOrEmpty(IdText.Text)
				|| String.IsNullOrEmpty(CaptchaAnswer.Text) || !int.TryParse(IdText.Text, out int x))
			{
				return;
			}
			if (!db.Users.Any(u=>u.Id == Convert.ToInt32(IdText.Text) && u.Password == PasswordText.Password))
			{
				MessageBox.Show("Пользователя с такими данными не существует");
				_counter++;
				CheckCounter();
				return;
			}
			if (CaptchaAnswer.Text != _answer)
			{
				MessageBox.Show("Неверно введена капча");
				_counter++;
				CheckCounter();
				return;
			}
			User user = db.Users.Include(u=>u.Role).Where(u => u.Id == Convert.ToInt32(IdText.Text) && u.Password == PasswordText.Password).FirstOrDefault();
			UserWindow win = new UserWindow(user);

			switch (user.Role.Id)
			{
				case 1:
					win.Show();
					break;
				case 2:
					win.Show();
					break;
				case 3:
					OrganizerWindow OrgWin = new OrganizerWindow(user);
					OrgWin.Show();
					break;
				case 4:
					win.Show();
					break;
			}
			this.DialogResult = true;
			this.Close();
		}
	}
}
