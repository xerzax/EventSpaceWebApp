using Domain.Entity.Base;
using EventSpaceApi.Domain.Entity.Identity;
using EventVerse.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Event
{
	public class Event: BaseEntity
	{
        public int Id { get; set; }

		/*[Required(ErrorMessage = "Event name is required")]
		[RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]*/
		public string? Name { get; set; }
		/*[Required(ErrorMessage = "Event venue is required")]*/
		public string? Venue {  get; set; }
		/*[Required(ErrorMessage = "Event date is required")]*/
		public DateTime Date {  get; set; }
        public string? Artist { get; set; }
		/*[Column(TypeName = "decimal(18,2)")]*/
		public decimal StartingPrice { get; set; }
        public int TotalSeats { get; set; }
        public int SoldSeats {  get; set; }
        public int AmountRaised { get; set;}
        public int AvailableSeats {  get; set; }
        public EventStatus Status { get; set; } = EventStatus.Open;
	/*	[Required(ErrorMessage = "Event type is required")]*/
		public EventType Type { get; set; }
        public Guid UserId {  get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

		public virtual ICollection<Tier> Tiers { get; set; }

		public virtual ICollection<EventWishlist> EventWishlists { get; set; }

		public string? EventPhoto { get; set; }

		[NotMapped]
		public IFormFile? PhotoUrl { get; set; }

	}
}
