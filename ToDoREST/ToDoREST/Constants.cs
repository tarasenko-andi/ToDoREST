using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ToDoREST
{
    public static class Constants
    {
        public enum SortedField { id, username, email, status }
        public enum SortDirection { asc, desc }
        // URL of REST service
        public static string developer = "?developer=NewAndi1";
        public static string Url = "https://uxcandy.com/~shapoval/test-task-backend/v2/";
        public static string StartUrl = Url + developer;
        public static string GetTasks(int page, SortedField sortedField, SortDirection sortDirection)
        {
            return StartUrl + "&sort_field=" + sortedField.ToString() + "&sort_direction=" + sortDirection.ToString() + "&page=" + page;
        }
        public static string AddTask = Url + "create" + developer;
        public static string Login = Url + "login" + developer;
        public static string UpdateTask(int id)
        {
            return Url + "edit/" + id + developer;
        }

    }
}
