using RedLinesDotterGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedLinesDotterGUI.Services;

namespace RedLinesDotterGUI.ViewModels
{
    class TablesViewModel : INotifyPropertyChanged
    {
        private Table selectedTable;

        public ObservableCollection<Table> Tables { get; set; }

        public Table SelectedTable 
        {
            get => selectedTable;
            set
            {
                selectedTable = value;
                OnPropertyChanged("SelectedTable");
            }
        }

        public TablesViewModel()
        {
            SetTables();            
        }

        public void SetTables()
        {
            var tabs = MapApplication.Instance.App.tables.Select(p => new Table() { Name = p.Name, Length = p.Lenght });
            Tables = new ObservableCollection<Table>(tabs);
        }

        private async Task GetTablesAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (MapApplication.Instance.IsConnected)
                    { 
                        var tabs = MapApplication.Instance.App.tables.Select(p => new Table() { Name = p.Name, Length = p.Lenght });
                        Tables = new ObservableCollection<Table>(tabs);
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                }                
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
