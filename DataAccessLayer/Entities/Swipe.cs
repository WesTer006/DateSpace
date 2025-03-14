using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Swipe
	{
		public int SwiperId { get; set; }
		public int TargetId { get; set; }
		public bool? TargetAgree { get; set; }  // NULL = второй не ответил, 1 = согласился, 0 = отказал
		public DateTime CreatedAt { get; set; }

		public AppUser Swiper { get; set; }
		public AppUser Target { get; set; }
	}
}
