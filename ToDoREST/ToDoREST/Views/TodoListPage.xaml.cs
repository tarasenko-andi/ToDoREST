﻿using System;
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
	public partial class TodoListPage : ContentPage
	{
		public ToDoListModelView viewModel { get; set; }
		public TodoListPage()
		{
			viewModel = new ToDoListModelView();
			BindingContext = viewModel;
			InitializeComponent();
            Appearing += TodoListPage_Appearing;
		}

        private void TodoListPage_Appearing(object sender, EventArgs e)
        {
			viewModel.UpdateItemsAsync();
        }

        async void OnAddItemClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TodoItemPage(true)
			{
				BindingContext = new ToDoItemModelView() { IsNew = true }
			}); ;

		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			listView.SelectedItem = null;
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			viewModel.AdminFormEnable = false;
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
			listView.SelectedItem = null;
			if (App.IsAdmin)
            {
				var item = ((Frame)sender).BindingContext as ToDoItemModelView;
				item.IsNew = false;
				await Navigation.PushAsync(new TodoItemPage
				{
					BindingContext = item
				});
			}
			else
				viewModel.AdminFormEnable = true;
		}
    }
}