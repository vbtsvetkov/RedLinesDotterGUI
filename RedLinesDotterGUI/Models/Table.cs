using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RedLinesDotterGUI.Models
{
    public sealed class Table : INotifyPropertyChanged
    {
        string name;
        int length;

        public string Name
        {
            get => name;
            set {
                name = value;
                OnPropertyChanged();
            }
        }

        public int Length
        {
            get => length;
            set {
                length = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
