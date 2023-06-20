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

        //Экземпляр полльзователя для добавления в базу данных
        private User newUser = new User();
        public User NewUser
        { 
            get
            {
                return newUser;
            }
            set
            {
                newUser = value;
                OnPropertyChange();
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

        //Реализация механизма добавления нового пользователя в базу данных
        #region
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                if(addCommand == null)
                {
                    addCommand = new RelayCommand(
                        param => AddNewItem());
                }
                return addCommand;
            }            
        }

        private void AddNewItem()
        {
            using(applicationContext = new ApplicationContext())
            {
                applicationContext.Users.Add(
                    new User()
                    {
                        Name = newUser.Name,
                        Email= newUser.Email
                    });
                applicationContext.SaveChanges();

                Users.Add(
                    new User()
                    {
                        ID = applicationContext.Users.ToList().Last().ID,
                        Name = newUser.Name,
                        Email = newUser.Email
                    });
            }
        }
        #endregion

        //Реализация механизма удаления выбраного в DataGrid пользователя из БД
        #region
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(
                        param => RemoveItem());
                }
                return removeCommand;
            }
        }

        private void RemoveItem()
        {
            using(applicationContext = new ApplicationContext())
            {
                if(SelectedUser != null)
                {
                    applicationContext.Users.Remove(SelectedUser);
                    applicationContext.SaveChanges();
                    Users.Remove(SelectedUser);
                }                
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
