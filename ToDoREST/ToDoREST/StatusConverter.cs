using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToDoREST.State;
using Xamarin.Forms;

namespace ToDoREST
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Status)value;
            var par = (string)parameter;
            if(par == "Execute")
            {
                if (val == Status.NoExecute)
                    return false;
                if (val == Status.Execute)
                    return true;
                if (val == Status.ExecuteAndAdminCheck)
                    return true;
                if (val == Status.NoExecuteAndAdminCheck)
                    return false;
            }
            if(par == "AdminEdit")
            {
                if (val == Status.NoExecute)
                    return false;
                if (val == Status.Execute)
                    return false;
                if (val == Status.ExecuteAndAdminCheck)
                    return true;
                if (val == Status.NoExecuteAndAdminCheck)
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var val = (bool)value;
            //var par = (string)parameter;
            //if (par == "Execute")
            //{
            //    return val ? Status.Execute : Status.NoExecute;
            //}
            //if (par == "AdminEdit")
            //{
            //    return val ? Status.ExecuteAndAdminCheck : Status.NoExecuteAndAdminCheck;
            //}
            return null;
        }
    }
}
