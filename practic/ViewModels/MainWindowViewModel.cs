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
	public class MainWindowViewModel: INotifyPropertyChanged
	{
		private Event selectedEvent;

		public IEnumerable<Event> allEvents { get; set; }
		public  List<string> Days { get; set; }
		private string selectedDay;

		public string SelectedDay
		{
			get { return selectedDay; }
			set
			{
				selectedDay = value;
				OnPropertyChanged(nameof(SelectedDay));
				OnPropertyChanged(nameof(Events));
			}
		}
		public IEnumerable<Event> Events
		{
			get
			{
				if (SelectedDay == "Все")
				{
					return allEvents;
				}
				else
				{
					return allEvents.Where(e=>e.Days == Convert.ToInt32(SelectedDay));
				}
			}
			set
			{
				allEvents = value;
				OnPropertyChanged(nameof(Events));
			}
		}

		public Event SelectedEvent
		{
			get { return selectedEvent; }
			set
			{
				selectedEvent = value;
				OnPropertyChanged("SelectedEvent");
			}
		}

		public MainWindowViewModel()
		{
			Context db = new Context();
			Days = new List<string>();
			Days.Add("Все");
			Days.AddRange((from e in db.Events
					group e by e.Days
					into d
					select d.Key.ToString()).ToList());
			SelectedDay = "Все";
			allEvents = db.Events.Include(e=>e.City).Include(e=>e.Winner).ToList();

		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
