using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoREST.Models;
using Xamarin.Forms;
using static ToDoREST.Constants;

namespace ToDoREST.Data
{
    public class RestService : IRestService
    {
        string Token { get; set; }
        HttpClient client;
        public static int _TimeoutSec = 30;
        public static string ServerLink = "https://blauberg-group-cloud.com/BL_Universal/";
        public static string _ContentType = "application/x-www-form-urlencoded";
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
                    pagesCount = message.total_task_count / 3;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return (pagesCount, Items);
        }

        public async Task SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
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
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
        }
        public async Task UpdateTodoItemAsync(TodoItem item)
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("text", item.text));
            nvc.Add(new KeyValuePair<string, string>("status", item.status?"10":"0"));
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Constants.AddTask) { Content = new FormUrlEncodedContent(nvc) };
                var res = client.SendAsync(req);
                status = await res.Result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
        }
        public async Task<bool> Logining(string login, string password)
        {
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
                    Token = response.message.token;
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
