using Microsoft.Win32;
using practic.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window
    {
		Context db;
		string _photo;
        public NewEventWindow()
        {
			db = new Context();
            InitializeComponent();
			GetValues();
        }
		private void GetValues()
		{
			IdText.Text = (db.Events.Max(x => x.Id)+1).ToString();
			City.ItemsSource = db.Cities.ToList();
			Date.DisplayDateStart = DateTime.Now.AddDays(1);

		}
		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog OFD = new OpenFileDialog();
			OFD.Filter = "Image files (*.png, *.jpg, *.jpeg, *.ico)|*.png;*.jpg;*.jpeg";
			if (OFD.ShowDialog() == true)
			{
				Uri fileUri = new Uri(OFD.FileName);
				string Name = IdText.Text;
				string Extension = System.IO.Path.GetExtension(OFD.FileName);

				PngBitmapEncoder PBE = new PngBitmapEncoder();
				PBE.Frames.Add(BitmapFrame.Create(new Uri(OFD.FileName)));
				using (FileStream FS = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Events\\" + Name + Extension, FileMode.Create))
					PBE.Save(FS);
				_photo = Name + Extension;
				Pfp.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Events", _photo)));
			}
		}
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(Name.Text.Trim())|| string.IsNullOrEmpty(Days.Text.Trim())||City.SelectedItem ==null
				|| Date.SelectedDate == null || !int.TryParse(Days.Text.Trim(), out int d))
			{
				MessageBox.Show("Проверьте поля");
			}
			else
			{
				Event @event = new Event
				{
					Name = Name.Text,
					Days = Convert.ToInt32(Days.Text),
					Date = (DateTime)Date.SelectedDate,
					City = City.SelectedValue as City,
				};
				db.Events.Add(@event);
				db.SaveChanges();
				MessageBox.Show("Мероприятие добавлено");
				this.Close();
			}
		}
	}
}
