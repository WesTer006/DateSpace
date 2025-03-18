using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Swipe
	{
		public required string SwiperId { get; set; }
		public required string TargetId { get; set; }
		public bool? TargetAgree { get; set; }  // NULL = second did not respond, 1 = agreed, 0 = refused
		public DateTime CreatedAt { get; set; }

		public required AppUser Swiper { get; set; }
		public required AppUser Target { get; set; }
	}
}
