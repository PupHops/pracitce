using Microsoft.EntityFrameworkCore;

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
using practic.ViewModels;
using System.Windows.Navigation;

namespace practic.Views
{
    /// <summary>
    /// Логика взаимодействия для ParticipantsWindow.xaml
    /// </summary>
    public partial class ParticipantsWindow : Window
    {
        Context db;
        public ParticipantsWindow()
        {
            db = new Context();
            InitializeComponent();
            DataContext = new ParticipantsWindowViewModel();
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            AddNewUser addNewUserWindow = new AddNewUser();
            addNewUserWindow.Show();
        }
    }
}
