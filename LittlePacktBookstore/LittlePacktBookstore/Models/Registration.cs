using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittlePacktBookstore.Models
{
	public class Registration
	{
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		[EmailAddress]
		//action method, home controller
		[Remote("CheckEmail", "Home", HttpMethod = "POST")]
		public string Email { get; set; }

		public Address MailingAddress { get; set; }
	}
}
