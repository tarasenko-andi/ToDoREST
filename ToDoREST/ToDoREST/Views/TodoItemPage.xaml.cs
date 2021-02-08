using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToDoREST.Models;
using ToDoREST.State;
using ToDoREST.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoREST.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoItemPage : ContentPage
	{
		bool isNewItem;
		ToDoItemModelView startedItem { get; set; }
		public TodoItemPage(bool isNew = false)
		{
			InitializeComponent();
			isNewItem = isNew;
            BindingContextChanged += TodoItemPage_BindingContextChanged;
		}
		bool updateContext;
        private void TodoItemPage_BindingContextChanged(object sender, EventArgs e)
        {
           if(!updateContext && BindingContext != null)
            {
				updateContext = true;
				var item = ((ToDoItemModelView)BindingContext).TodoItem;
				startedItem = new ToDoItemModelView(new TodoItem { email = item.email, id = item.id, status = item.status, text = item.text, username=item.username });
			}
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
		{
			var todoItem = (ToDoItemModelView)BindingContext;
			if (ValidEntry(todoItem, out string str))
            {
				await DisplayAlert("Ошибка ввода", str, "OK");
				return;
            }
            if (todoItem.IsNew)
            {
				if (await App.TodoManager.SaveTaskAsync(todoItem.TodoItem, isNewItem))
				{
					await DisplayAlert("Успешно", "Сохранение данных прошло успешно", "ОК");
					return;
				}
				else
					await DisplayAlert("Ошибка", "Ошибка сохранения данных", "ОК");
            }
            else
            {
				ToDoItemModelView.ChangeStatus(todoItem, todoItem.Text != startedItem.Text , todoItem.Execute);
				await App.TodoManager.UpdateTodoItemAsync(todoItem.TodoItem);
			}
			await Navigation.PopAsync();
		}

        private bool ValidEntry(ToDoItemModelView item, out string str)
        {
			str = "";
			if(item.UserName == null || item.UserName.Length == 0)
            {
				str += "Поле имья не заполнено";
            }
			if(item.Email == null || !Regex.IsMatch(item.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
			{
				str += "Поле Email не соответсвует патерну";
			}
			if(item.Text == null || item.Text.Length == 0)
            {
				str += "Поле Задачи не заполнено";
			}
			return str != "";
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}