using System;
using ToDoREST.Data;
using ToDoREST.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoREST
{
    public partial class App : Application
    {
        public static bool IsAdmin { get; set; }
        public static TodoItemManager TodoManager { get; private set; }
        public App()
        {
            TodoManager = new TodoItemManager(new RestService());
            MainPage = new NavigationPage(new TodoListPage());
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
