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

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChange("Name");
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChange("Email");
            }
        }

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
                        Name = this.Name,
                        Email= this.Email
                    });
                applicationContext.SaveChanges();

                Users.Add(
                    new User()
                    {
                        ID = applicationContext.Users.ToList().Last().ID,
                        Name = this.Name,
                        Email = this.Email
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

        //Реализация механизма изменения данных выбраного в DataGrid пользователя
        #region
        private RelayCommand modifyCommand;
        public RelayCommand ModifyCommand
        {
            get
            {
                if(modifyCommand == null)
                {
                    modifyCommand = new RelayCommand(
                        param => ModifyItem());
                }
                return modifyCommand;
            }
        }

        private void ModifyItem()
        {
            using(applicationContext = new ApplicationContext())
            {
                if(SelectedUser != null)
                {                    
                    var user = applicationContext.Users.FirstOrDefault(s => s.ID == SelectedUser.ID);                    

                    if (user != null)
                    {
                        user.Name = this.Name;
                        user.Email = this.Email;
                        applicationContext.SaveChanges();
                    }
                    
                    var userObservableCollection = Users.FirstOrDefault(s => s.ID == SelectedUser.ID);

                    if (userObservableCollection != null)
                    {
                        userObservableCollection.Name = this.Name;
                        userObservableCollection.Email = this.Email;
                    }                    
                }
            }
        }
        #endregion

        //Реализация обновления textBox's при выборе пользователя в DataGrid
        #region
        private RelayCommand updateTextBoxesCommand;
        public RelayCommand UpdateTextBoxesCommand
        {
            get
            {
                if(updateTextBoxesCommand == null)
                {
                    updateTextBoxesCommand = new RelayCommand(
                        param => UpdateTextBoxs());
                }
                return updateTextBoxesCommand;
            }
        }

        private void UpdateTextBoxs()
        {
            if(SelectedUser != null)
            {
                this.Name = selectedUser.Name;
                this.Email = selectedUser.Email;
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
