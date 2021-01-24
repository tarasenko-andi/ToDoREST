using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoREST.Data
{
    public class AddItemResponse
    {
            public string status { get; set; }
            public Message message { get; set; }
            public AddItemResponse()
            {

            }

        public class Message
        {
        }
    }
}
