using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
	class DBUtil
	{
		public static DEB2Context db;

		static DBUtil()
		{
			db = new DEB2Context();
		}

		~DBUtil()
		{
			db.Dispose();
		}
	}
}
