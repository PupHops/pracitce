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
using System.Windows.Controls.Primitives;


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
        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            int currentYear = DateTime.Today.Year; 
            int startYear = currentYear - 60; 
            int endYear = currentYear - 10;

            datePicker.DisplayDateStart = new DateTime(startYear, 1, 1);
            datePicker.DisplayDateEnd = new DateTime(endYear, 12, 31);
        }
        private void Birthday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            int currentYear = DateTime.Today.Year;
            int startYear = currentYear - 60;
            int endYear = currentYear - 10;

            // Проверяем, что выбранная дата входит в указанный промежуток
            if (selectedDate.Year < startYear || selectedDate.Year > endYear)
            {
                MessageBox.Show("Пожалуйста, введите корректную дату рождения");
                datePicker.SelectedDate = null; // Очищаем выбранную дату
            }
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
            List<string> errors = new List<string>();

            if (String.IsNullOrEmpty(Name.Text))
                errors.Add("Имя не указано");

            if (String.IsNullOrEmpty(Surname.Text))
                errors.Add("Фамилия не указана");

            if (String.IsNullOrEmpty(Email.Text))
                errors.Add("Email не указан");
            else if (!Email.Text.Contains("@"))
                errors.Add("Некорректный формат Email");
            else if (!Regex.IsMatch(Email.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                errors.Add("Email должен содержать только английские буквы и соответствовать формату");

            if (String.IsNullOrEmpty(Phonenumber.Text))
                errors.Add("Номер телефона не указан");
            else if (new Regex(@"\D").IsMatch(Phonenumber.Text))
                errors.Add("Номер телефона должен содержать только цифры");
            else if (Phonenumber.Text.Length < 9)
                errors.Add("Номер телефона должен содержать не менее 9 символов");
            else if (Phonenumber.Text.All(c => c == '0'))
                errors.Add("Номер телефона не может состоять только из нулей");

            if (Gender.SelectedItem == null)
                errors.Add("Пол не выбран");

            if (Role.SelectedItem == null)
                errors.Add("Роль не выбрана");

            if (String.IsNullOrEmpty(Password.Password))
                errors.Add("Пароль не указан");
            else if (Password.Password.Length < 6)
                errors.Add("Пароль должен содержать не менее 6 символов");
            else if (!Password.Password.Any(char.IsUpper))
                errors.Add("Пароль должен содержать хотя бы одну заглавную букву");
            else if (!Password.Password.Any(char.IsLower))
                errors.Add("Пароль должен содержать хотя бы одну строчную букву");
            else if (!Password.Password.Any(ch => !char.IsLetterOrDigit(ch)))
                errors.Add("Пароль должен содержать хотя бы один специальный символ");

            if (String.IsNullOrEmpty(PasswordCheck.Password))
                errors.Add("Подтверждение пароля не указано");

            if (Password.Password != PasswordCheck.Password)
                errors.Add("Пароли не совпадают");

            if (Country.SelectedItem == null)
                errors.Add("Страна не выбрана");

            if (Course.SelectedItem == null)
                errors.Add("Курс не выбран");

            if (Birthday.SelectedDate == null)
                errors.Add("Дата рождения не выбрана");

            if (!Regex.IsMatch(Email.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                errors.Add("Поле имейл должно содержать только английские буквы и соответствовать формату");
            

            string[] allowedEndings = { ".com", ".ru", ".net", ".org", ".edu", ".gov", ".info", ".biz", ".us" }; // Добавлены дополнительные окончания
            bool validEnding = false;
            foreach (string ending in allowedEndings)
            {
                if (Email.Text.EndsWith(ending))
                {
                    validEnding = true;
                    break;
                }
            }
            if (!validEnding)
            {
                errors.Add("Поле имейл должно оканчиваться на одно из распространенных окончаний: .com, .ru, .net, .org, .edu, .gov, .info, .biz, .us");
            }

            if (errors.Count > 0)
            {
                string errorMessage = "Обнаружены следующие ошибки:\n\n";
                errorMessage += string.Join("\n", errors);
                MessageBox.Show(errorMessage);
            }
            else
            {
                // Все данные заполнены корректно, продолжаем обработку
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
                    if (Role.SelectedValue == db.Roles.FirstOrDefault(x => x.Id == 2) && (Activity.SelectedValue as Activity).Moderator != null)
                    {
                        MessageBox.Show("У данной активности уже есть модератор");
                    }
                    else
                    {
                        db.Users.Add(user);
                        if (Activity.SelectedValue != null)
                            (Activity.SelectedValue as Activity).Moderator = user;
                        MessageBox.Show("Пользователь добавлен");
                        this.Close();
                    }

                    if (Role.SelectedValue == db.Roles.FirstOrDefault(x => x.Id == 4))
                    {
                        db.Users.Add(user);
                        if (Activity.SelectedValue != null)
                            db.ActivityJuries.Add(new ActivityJury
                            {
                                Activity = Activity.SelectedValue as Activity,
                                Jurie = user
                            });
                        MessageBox.Show("Пользователь добавлен");
                        this.Close();
                    }
                }
                else
                {
                    db.Users.Add(user);
                    MessageBox.Show("Пользователь добавлен");
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
