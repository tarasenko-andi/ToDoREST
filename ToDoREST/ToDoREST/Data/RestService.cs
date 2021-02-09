using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoREST.Models;
using ToDoREST.State;
using Xamarin.Essentials;
using Xamarin.Forms;
using static ToDoREST.Constants;

namespace ToDoREST.Data
{
    public class RestService : IRestService
    {
        HttpClient client;
        public static int _TimeoutSec = 30;
        public static string ServerLink = "https://blauberg-group-cloud.com/BL_Universal/";
        public static string _ContentType = "multipart/form-data";
        public static string _UserAgent = "d-fens HttpClient";
        public static string status = "no determinate";//начальный статус

        public List<TodoItem> Items { get; private set; }

        public RestService()
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            client.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
        }

        public async Task<(int pagesCount, List<TodoItem>)> RefreshDataAsync(int page, SortedField sortedField, SortDirection sortDirection)
        {
            Items = new List<TodoItem>();
            int pagesCount = 0;
            string str = Constants.GetTasks(page, sortedField, sortDirection);
            try
            {
                Uri uri = new Uri(string.Format(str, string.Empty));
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var message = JsonConvert.DeserializeObject<JSONResponse>(content).message;
                    Items = message.tasks;
                    if (message.total_task_count % 3 == 0)
                        pagesCount = (message.total_task_count / 3);
                    else
                        pagesCount = (message.total_task_count / 3) + 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return (pagesCount, Items);
        }

        public async Task<bool> SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("username", item.username));
            nvc.Add(new KeyValuePair<string, string>("email", item.email));
            nvc.Add(new KeyValuePair<string, string>("text", item.text));
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Constants.AddTask) { Content = new FormUrlEncodedContent(nvc) };
                var res = client.SendAsync(req);
                status = await res.Result.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AddItemResponse>(status).status;
                return result == "ok";
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }
        public async Task<bool> UpdateTodoItemAsync(TodoItem item)
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("token", Preferences.Get("token", "")));
            nvc.Add(new KeyValuePair<string, string>("text", item.text));
            nvc.Add(new KeyValuePair<string, string>("status", ((int)(item.status)).ToString()));
            try
            {
                string url = Constants.UpdateTask(item.id);
                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };
                var res = client.SendAsync(req);
                status = await res.Result.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AddItemResponse>(status).status;
                return result == "Ok";
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }
        public class Token
        {
            public string token { get; set; }
            public DateTime Time { get; set; }
            public bool IsOut { get; set; }
        }
        public async Task<bool> Logining(string login, string password)
        {
            DateTime datetimetoken = Preferences.Get("datetimetoken", default(DateTime));
            string token = Preferences.Get("token", "");
            bool isout = Preferences.Get("isout", false);
            if (token != "")
            {
                if (!isout)
                {
                    int year = DateTime.Now.Year - datetimetoken.Year;
                    int month = DateTime.Now.Month - datetimetoken.Month;
                    int day = DateTime.Now.Day - datetimetoken.Day;
                    int hour = DateTime.Now.Hour - datetimetoken.Hour;
                    if (year == 0 && month == 0 && day == 0 && hour < 23)
                        return true;
                }
            }
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("username", login));
            nvc.Add(new KeyValuePair<string, string>("password", password));
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Constants.Login) { Content = new FormUrlEncodedContent(nvc) };
                var res = client.SendAsync(req);
                status = await res.Result.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<AdminResponse>(status);
                if (response.status == "ok")
                {
                    Preferences.Set("token", response.message.token);
                    Preferences.Set("isout", false);
                    Preferences.Set("datetimetoken", DateTime.Now);
                    Preferences.Set("username", login);
                    Preferences.Set("password", password);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }

        }
    }
}
