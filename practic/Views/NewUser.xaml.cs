using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Win32;
using System.IO;
using practic.Models;
using System.Text.RegularExpressions;

namespace practic.Views
{
    /// <summary>
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        Context db;
        string _photo;
        public NewUser()
        {
            db = new Context();
            InitializeComponent();
            Birthday.DisplayDateEnd = DateTime.Now;
            GetValues();
        }
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
            this.Close();
        }
        private void GetValues()
        {
            IdText.Text = (db.Users.Max(x => x.Id) + 1).ToString();
            Event.ItemsSource = db.Events.ToList();
            Gender.ItemsSource = new List<char>{'м','ж'};
            Role.ItemsSource = db.Roles.Where(x=>x.Id == 2 || x.Id == 4).ToList();
            Country.ItemsSource = db.Countries.ToList();
            Course.ItemsSource = db.Course.ToList();
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Image files (*.png, *.jpg, *.jpeg, *.ico)|*.png;*.jpg;*.jpeg";
            if (OFD.ShowDialog() == true)
            {
                Uri fileUri = new Uri(OFD.FileName);
                string Name = System.IO.Path.GetRandomFileName();
                string Extension = System.IO.Path.GetExtension(OFD.FileName);

                PngBitmapEncoder PBE = new PngBitmapEncoder();
                PBE.Frames.Add(BitmapFrame.Create(new Uri(OFD.FileName)));
                using (FileStream FS = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Users\\" + Name + Extension, FileMode.Create))
                    PBE.Save(FS);
                _photo = Name+Extension;
                Pfp.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Users", _photo)));
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Name.Text)|| String.IsNullOrEmpty(Surname.Text)|| String.IsNullOrEmpty(Email.Text)||String.IsNullOrEmpty(Phonenumber.Text)
                ||Gender.SelectedItem==null|| Role.SelectedItem == null || new Regex(@"\D").Matches(Phonenumber.Text).Count()>0 || String.IsNullOrEmpty(Password.Password)
                || String.IsNullOrEmpty(PasswordCheck.Password) || Password.Password != PasswordCheck.Password || Phonenumber.Text.Length<9
                ||Country.SelectedItem == null || Course.SelectedItem == null || Birthday.SelectedDate == null)
            {
                MessageBox.Show("Проверьте поля");
            }
            else
            {
                User user = new User
                {
                    Name = Name.Text,
                    Surname = Surname.Text,
                    Email = Email.Text,
                    Patronimic = Patronimic.Text,
                    Birthday = (DateTime)Birthday.SelectedDate,
                    Password = Password.Password,
                    Photo = _photo,
                    Country = Country.SelectedValue as Country,
                    Course = Course.SelectedValue as Course,
                    Role = Role.SelectedValue as Role,
                    Gender = (char)Gender.SelectedValue,
                    Phone = Phonenumber.Text
                };
                if (Activity.SelectedValue != null)
                {
                    if (Role.SelectedValue == db.Roles.Where(x => x.Id == 2).FirstOrDefault() && (Activity.SelectedValue as Activity).Moderator != null)
                    {
                        MessageBox.Show("У данной активности уже есть модератор");
                    }
                    else
                    {
                        db.Users.Add(user);
                        if (Activity.SelectedValue != null)
                            (Activity.SelectedValue as Activity).Moderator = user;
                        MessageBox.Show("Пользователь добален");
                        this.Close();
                    }

                    if (Role.SelectedValue == db.Roles.Where(x => x.Id == 4).FirstOrDefault())
                    {
                        db.Users.Add(user);
                        if (Activity.SelectedValue != null)
                            db.ActivityJuries.Add(new ActivityJury
                            {
                                Activity = Activity.SelectedValue as Activity,
                                Jurie = user
                            });
                        MessageBox.Show("Пользователь добален");
                        this.Close();
                    }
                }
                else
                {
                    db.Users.Add(user);
                    MessageBox.Show("Пользователь добален");
                    this.Close();
                } 
                db.SaveChanges();
            }
        }

        private void Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                Activity.ItemsSource = db.ActivityEvents
                .Include(ae => ae.Event)
                .Include(ae => ae.Activity)
                .ThenInclude(a=>a.Moderator)
                .Where(ae => ae.Event == Event.SelectedValue)
                .Select(ae => ae.Activity).ToList();
        }
    }
}
