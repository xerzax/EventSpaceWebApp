using Application.DTOs.Email;
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

		private  EmailOption _emailOption { get; }
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
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
