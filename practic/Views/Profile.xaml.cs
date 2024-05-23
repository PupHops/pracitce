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
using practic.Models;

namespace practic.Views
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        Context db;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Surname.Text) ||
                string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Phone.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!IsValidName(Name.Text) || !IsValidName(Surname.Text) || !IsValidName(Patronimic.Text))
            {
                MessageBox.Show("Имя, фамилия и отчество должны содержать только буквы.");
                return;
            }

            if (ContainsLetters(Phone.Text))
            {
                MessageBox.Show("Номер телефона не должен содержать буквы.");
                return;
            }

            // Проверка на наличие множественных нулей в номере телефона
            if (Phone.Text.Contains("000") || Phone.Text.All(c => c == '0'))
            {
                MessageBox.Show("Номер телефона не может состоять только из нулей или иметь много нулей подряд.");
                return;
            }

            if (!IsValidEmail(Email.Text))
            {
                MessageBox.Show("Неверный формат адреса электронной почты.");
                return;
            }

            User.Name = Name.Text;
            User.Surname = Surname.Text;
            User.Patronimic = Patronimic.Text;
            User.Email = Email.Text;
            User.Phone = Phone.Text;

            using (var dbContext = new Context())
            {
                dbContext.Users.Attach(User);
                dbContext.Entry(User).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            MessageBox.Show("Данные сохранены");
            this.Close();
        }


        private bool IsValidEmail(string email)
        {
            // Проверка наличия @ в адресе
            if (!email.Contains("@"))
                return false;

            // Проверка окончания адреса на допустимые варианты
            string[] allowedEndings = { ".com", ".ru", ".net", ".org", ".edu", ".gov", ".info", ".biz", ".us" };
            bool validEnding = false;
            foreach (string ending in allowedEndings)
            {
                if (email.EndsWith(ending))
                {
                    validEnding = true;
                    break;
                }
            }
            if (!validEnding)
                return false;



            return true;
        }

        private bool IsValidName(string text)
        {
            return !string.IsNullOrEmpty(text) && text.All(char.IsLetter);
        }

        private bool ContainsDigits(string input)
        {
            return input.Any(char.IsDigit);
        }

        private bool ContainsLetters(string input)
        {
            return input.Any(char.IsLetter);
        }
    }
}
