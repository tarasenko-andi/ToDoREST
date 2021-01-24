using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoREST.Data
{
    public class AdminResponse
    {
        public string status { get; set; }
        public Message1 message { get; set; }
        public AdminResponse()
        {

        }
    }
    public class Message1
    {
        public string token { get; set; }
        public Message1()
        {

        }
    }
}
