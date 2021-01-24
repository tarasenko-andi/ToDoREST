using System;
using System.Collections.Generic;
using System.Text;
using ToDoREST.Models;

namespace ToDoREST.Data
{
    public class JSONResponse
    {
        public string status { get; set; }
        public Message message { get; set; }
        public JSONResponse()
        {

        }
    }

    public class Message
    {
        public List<TodoItem> tasks { get; set; }
        public int total_task_count { get; set; }
        public Message()
        {

        }
    }
}
