using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoREST.Models;
using Xamarin.Forms;
using static ToDoREST.Constants;
using static ToDoREST.Data.RestService;

namespace ToDoREST.ViewModels
{
    public class ToDoListModelView : INotifyPropertyChanged
    {
        public Page CallPage { get; set; }
        bool isAdmin = App.IsAdmin;
        public bool IsAdmin { get => isAdmin;
            set
            {
                if(isAdmin != value)
                {
                    isAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                }
            }
        }
        public Command LoginingCommand { get; set; }
        List<ToDoItemModelView> items;
        public List<ToDoItemModelView> Items { get { return items; }
            set 
            {
                if(items!= value)
                {
                    items = value;
                    OnPropertyChanged(nameof(Items));
                }
            } 
        }
        public ToDoListModelView(Page page)
        {
            CallPage = page;
            LoginingCommand = new Command(async() => await Logining());
            ChangeSortedTypeCommand = new Command((object type) => ChangeSortedType(type));
            Items = new List<ToDoItemModelView>();
            CurrentSortedField = SortedField.username;
            CurrentPage = 1;
            PagesCheckList = new List<int>() { CurrentPage };
            Task.Run(() => UpdateItemsAsync());
            App.UpdateIsAdmin += App_UpdateIsAdmin;
            SavedList = new List<ToDoItemModelView>();
        }

        internal void UnLoging()
        {
            App.IsAdmin = false;
            if (App.Current.Properties.TryGetValue("username", out object user))
                App.Current.Properties["username"] = "";
            if (App.Current.Properties.TryGetValue("password", out object password))
                App.Current.Properties["password"] = "";
            if (App.Current.Properties.TryGetValue("newtoken", out object token))
                App.Current.Properties["newtoken"] = new Token() { IsOut = true };
        }

        private void App_UpdateIsAdmin(bool state)
        {
            IsAdmin = state;
        }

        string login = "admin";
        public string Login { get => login;
            set
            {
                if(login != value)
                {
                    login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }
        bool adminFormEnable;
        public bool AdminFormEnable 
        {
            get => adminFormEnable;
            set
            {
                if(adminFormEnable != value)
                {
                    adminFormEnable = value;
                    OnPropertyChanged(nameof(AdminFormEnable));
                }
            }
        }
        string password = "123";
        public string Password { get => password; 
            set
            {
                if(password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        private async Task Logining()
        {
            string valid = ValidateModel();
            if(valid != "")
            {
                await CallPage.DisplayAlert("Ошибка ввода данных", valid, "OK");
                return;
            }
            App.IsAdmin = await App.TodoManager.Logining(Login, Password);
            AdminFormEnable = false;
            if (App.IsAdmin)
                await CallPage.DisplayAlert("Успешно", "Вы АДМИН!!!", "ОК");
            else
                await CallPage.DisplayAlert("Ошибка", "Не удалось войти в аккаунт, проверьте интернет соединение и соответсвие логина и пароля", "ОК");
        }

        private string ValidateModel()
        {
            string valid = "";
            if(Login == null || Login.Length == 0)
            {
                valid += "Поле логин не заполнено";
            }
            if(Password == null || Password.Length == 0)
            {
                valid += "Поле  не заполнено";
                valid += "Поле пароль не заполнено";
            }
            return valid;
        }

        int pagesCount;
        public int PagesCount { get => pagesCount;
            set
            {
                if(pagesCount != value)
                {
                    pagesCount = value;
                    OnPropertyChanged(nameof(PagesCount));
                    UpdateCheckedPageList();
                }
            }
        }

        private void UpdateCheckedPageList()
        {
            List<int> newList = new List<int>();
            for (int i = 1; i <= PagesCount; i++)
            {
                newList.Add(i);
            }
            PagesCheckList = newList;
        }

        private void ChangeSortedType(object type)
        {
            var sortedField = ((SortedField)Int32.Parse((string)type));
            if (sortedField == CurrentSortedField)
                CurrentSortDirection = CurrentSortDirection == SortDirection.asc ? SortDirection.desc : SortDirection.asc;
            else
                CurrentSortedField = sortedField;
            ChangeState = Int32.Parse(((int)CurrentSortedField).ToString() + ((int)CurrentSortDirection).ToString());
        }
        int changeState;
        public int ChangeState { get => changeState;
            set
            {
                if(changeState != value)
                {
                    changeState = value;
                    OnPropertyChanged(nameof(ChangeState));
                }
            }
        }
        public ICommand ChangeSortedTypeCommand { get; set; }
        int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                if(currentPage != value)
                {
                    currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    Task.Run(() => UpdateItemsAsync());
                }
            }
        }
        SortedField sortedField;
        public SortedField CurrentSortedField { get => sortedField;
            set
            {
                if(sortedField != value)
                {
                    sortedField = value;
                    OnPropertyChanged(nameof(this.CurrentSortedField));
                    Task.Run(() => UpdateItemsAsync());
                }
            }
        }
        SortDirection sortDirection;
        public SortDirection CurrentSortDirection
        {
            get => sortDirection;
            set
            {
                if (sortDirection != value)
                {
                    sortDirection = value;
                    OnPropertyChanged(nameof(this.CurrentSortDirection));
                    Task.Run(() => UpdateItemsAsync());
                }
            }
        }
        List<int> pagesCheckList;
        public List<int> PagesCheckList { get => pagesCheckList;
            set
            {
                if(pagesCheckList != value)
                {
                    pagesCheckList = value;
                    OnPropertyChanged(nameof(PagesCheckList));
                }
            }
        }
        List<ToDoItemModelView> SavedList { get; set; }
        bool isBussy;
        public async Task UpdateItemsAsync()
        {
            if (!isBussy)
            {
                isBussy = true;
                (int pCount, List<TodoItem> todoItems) = await App.TodoManager.GetTasksAsync(CurrentPage, CurrentSortedField, CurrentSortDirection);
                var newItems = new List<ToDoItemModelView>();
                foreach (var item in todoItems)
                {
                    if(Items.FirstOrDefault(x=>x.ID == item.id) == null)
                    {
                        var newItem = new ToDoItemModelView(item);
                        newItems.Add(newItem);
                        SavedList.Add(newItem);
                    }
                    else
                    {
                        newItems.Add(SavedList.FirstOrDefault(x => x.ID == item.id));
                    }
                }
                Items = newItems;
                PagesCount = pCount;
                isBussy = false;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
