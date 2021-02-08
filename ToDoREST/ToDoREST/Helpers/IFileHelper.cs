using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoREST.Helpers
{
	public interface IFileHelper
	{
		string GetLocalFilePath(string filename);
	}
}
