using Entity_Framework_WPF.DB_Context;
using Entity_Framework_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_WPF.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //Экземпляр класса-контекста для взаимодействия с базой данных
        ApplicationContext applicationContext;
        //Выбранный пользователь
        private User selectedUser;
        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                OnPropertyChange(nameof(SelectedUser));
            }
        }

        //Коллекция пользовантелей полученных из базы данных, для привязки к DataGrid
        public ObservableCollection<User> Users { get; set; }

        public MainViewModel()
        {
            applicationContext = new ApplicationContext();
            Users = new ObservableCollection<User>(applicationContext.Users);
            applicationContext.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
