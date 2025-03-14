using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Message
	{
		public int Id { get; set; }
		public int SenderId { get; set; }
		public int ReceiverId { get; set; }
		public string MessageText { get; set; }
		public DateTime CreatedAt { get; set; }

		public AppUser Sender { get; set; }
		public AppUser Receiver { get; set; }
	}
}
