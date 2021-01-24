using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoREST.Models;
using ToDoREST.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoREST.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoItemPage : ContentPage
	{
		bool isNewItem;

		public TodoItemPage(bool isNew = false)
		{
			InitializeComponent();
			isNewItem = isNew;
		}

		async void OnSaveButtonClicked(object sender, EventArgs e)
		{
			var todoItem = (ToDoItemModelView)BindingContext;
			await App.TodoManager.SaveTaskAsync(todoItem.TodoItem, isNewItem);
			await Navigation.PopAsync();
		}

		async void OnCancelButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}