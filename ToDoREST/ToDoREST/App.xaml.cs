using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoREST.Data;
using ToDoREST.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoREST
{
    public partial class App : Application
    {
        static bool isAdmin;
        public static bool IsAdmin { get => isAdmin;
            set
            {
                if (isAdmin != value)
                {
                    isAdmin = value;
                    UpdateIsAdmin?.Invoke(value);
                }
            } 
        }
        public delegate void UpdateIsAdminEventHandler(bool state);
        public static event UpdateIsAdminEventHandler UpdateIsAdmin;
        public static TodoItemManager TodoManager { get; private set; }
        public App()
        {
            TodoManager = new TodoItemManager(new RestService());
            MainPage = new NavigationPage(new TodoListPage());
            if (App.Current.Properties.TryGetValue("username", out object user) && App.Current.Properties.TryGetValue("password", out object password))
                Task.Run(() => App.IsAdmin = TodoManager.Logining((string)user, (string)password).Result);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
