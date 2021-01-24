using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoREST.Models;
using Xamarin.Forms;
using static ToDoREST.Constants;

namespace ToDoREST.ViewModels
{
    public class ToDoListModelView : INotifyPropertyChanged
    {
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
        public ToDoListModelView()
        {
            LoginingCommand = new Command(async() => await Logining());
            ChangeSortedTypeCommand = new Command((object type) => ChangeSortedType(type));
            Items = new List<ToDoItemModelView>();
            CurrentSortedField = SortedField.username;
            CurrentPage = 1;
            PagesCheckList = new List<int>() { CurrentPage };
            Task.Run(() => UpdateItemsAsync());
        }
        string login;
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
        string password;
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
            App.IsAdmin = await App.TodoManager.Logining(Login, Password);
            AdminFormEnable = false;
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
            PagesCheckList = new List<int>();
            for (int i = 1; i <= PagesCount; i++)
            {
                PagesCheckList.Add(i);
            }
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

        public async Task UpdateItemsAsync()
        {
            (int pCount, List<TodoItem> todoItems) = await App.TodoManager.GetTasksAsync(CurrentPage, CurrentSortedField, CurrentSortDirection);
            var newItems = new List<ToDoItemModelView>();
            foreach (var item in todoItems)
            {
                newItems.Add(new ToDoItemModelView(item));
            }
            Items = newItems;
            PagesCount = pCount;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
