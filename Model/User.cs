using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_WPF.Model
{
    public class User : INotifyPropertyChanged
    {        
        private string name;
        private string email;

        public int ID { get; set; }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                value = name;
                OnPropertyChanged("Name");
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                value = email;
                OnPropertyChanged("Email");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop ="")
        {
            if(PropertyChanged != null)            
               PropertyChanged(this, new PropertyChangedEventArgs(prop));            
        }
    }
}
