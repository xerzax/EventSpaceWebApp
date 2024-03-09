using Application.DTOs.Email;
using Application.DTOs.Ticket;
using Application.Interfaces.Services;
using Domain.Constants;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class EmailService : IEmailService

	{

		private EmailOption _emailOption { get; }
		public EmailService(IOptions<EmailOption> emailSettings)
		{
			_emailOption = emailSettings.Value;
		}
		public async Task SendEmail(EmailActionDto emailAction)
		{
			try
			{
				var client = new MailjetClient(_emailOption.ApiKey, _emailOption.SecretKey);

				emailAction.Body = $"Please confirm email by clicking this link: <a href=\"{emailAction.Url}\">link</a>";

				var request = new MailjetRequest
				{
					Resource = Send.Resource,
				}.Property(Send.Messages, new JArray {
				new JObject {
					{ "FromEmail", "ritika.shrestha707@gmail.com" },
					{ "FromName", "EventSpace" },
					{ "Recipients", new JArray {
						new JObject {
							{ "Email", emailAction.Email },
							{ "Name", emailAction.Email }
						}
					}},
					{ "Subject", emailAction.Subject },
					{ "Text-part", "Dear passenger, welcome to Mailjet! May the delivery force be with you!" },
					{ "Html-part", emailAction.Body }
				}
			});

				MailjetResponse response = await client.PostAsync(request);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public async Task SendTicketPurchaseConfirmationEmail(TicketResponseDTO ticketResponseDTO, string userEmail)
		{
			try
			{
				var client = new MailjetClient(_emailOption.ApiKey, _emailOption.SecretKey);
				string emailBody = $"Thank you for purchasing the tickets! Here are the details:<br/><br/>" +
								   $"Quantity: {ticketResponseDTO.Qty}<br/>" +
								   $"Total Price: {ticketResponseDTO.TotalPrice}<br/>" +
								   $"Tier: {ticketResponseDTO.TierName}<br/>" +
								   $"Event Type: {ticketResponseDTO.TicketType}<br/>" +
								   $"Venue: {ticketResponseDTO.Venue}<br/>" +
								   $"Event Date: {ticketResponseDTO.Eventdate}<br/><br/>" +
								   "We appreciate your support!";

				var request = new MailjetRequest
				{
					Resource = Send.Resource,
				}.Property(Send.Messages, new JArray {
			new JObject {
				{ "FromEmail", "ritika.shrestha707@gmail.com" },
				{ "FromName", "EventSpace" },
				{ "Recipients", new JArray {
					new JObject {
						{ "Email", userEmail },
						{ "Name", userEmail }
					}
				}},
				{ "Subject", "Ticket Purchase Confirmation" },
				{ "Text-part", "Thank you for purchasing the tickets! We appreciate your support." },
				{ "Html-part", emailBody }
			}
		});

				MailjetResponse response = await client.PostAsync(request);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
