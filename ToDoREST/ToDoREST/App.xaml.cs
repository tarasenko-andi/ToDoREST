using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoREST.Data;
using ToDoREST.Views;
using Xamarin.Essentials;
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
            string username = Preferences.Get("username", "noUser");
            string password = Preferences.Get("password", "noPassword");
            if (username != "noUser" && password != "noPassword")
                Task.Run(() => App.IsAdmin = TodoManager.Logining(username, password).Result);
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
