using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Preference
	{
		public int UserId { get; set; }
		public string PreferredGender { get; set; }
		public int MinAge { get; set; }
		public int MaxAge { get; set; }
		public int MaxDistance { get; set; }

		public AppUser User { get; set; }
	}
}
