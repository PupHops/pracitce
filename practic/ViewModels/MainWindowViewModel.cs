using practic.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows;

namespace practic.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Event selectedEvent;
        private IEnumerable<Event> allEvents;
        private List<string> sortOptions = new List<string> { "Нет сортировки", "По возрастанию даты", "По убыванию даты" };
        private string selectedSortOption = "Нет сортировки";

        public IEnumerable<Event> AllEvents
        {
            get { return allEvents; }
            set
            {
                allEvents = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        public List<string> SortOptions
        {
            get { return sortOptions; }
            set
            {
                sortOptions = value;
                OnPropertyChanged(nameof(SortOptions));
            }
        }

        public string SelectedSortOption
        {
            get { return selectedSortOption; }
            set
            {
                selectedSortOption = value;
                OnPropertyChanged(nameof(SelectedSortOption));
                OnPropertyChanged(nameof(Events));
            }
        }

        public IEnumerable<Event> Events
        {
            get
            {
                IEnumerable<Event> eventsToShow = AllEvents;

                if (SelectedSortOption == "По возрастанию даты")
                {
                    eventsToShow = eventsToShow.OrderBy(e => e.Date);
                }
                else if (SelectedSortOption == "По убыванию даты")
                {
                    eventsToShow = eventsToShow.OrderByDescending(e => e.Date);
                }

                return eventsToShow;
            }
        }

        public Event SelectedEvent
        {
            get { return selectedEvent; }
            set
            {
                selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }

        public MainWindowViewModel()
        {
            Context db = new Context();
            AllEvents = db.Events.Include(e => e.City).Include(e => e.Winner).Include(e => e.ActivityEvents).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
