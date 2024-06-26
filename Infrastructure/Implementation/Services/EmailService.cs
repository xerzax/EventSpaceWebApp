﻿using Application.DTOs.Email;
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
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing.QrCode.Internal;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Drawing;
using EventVerse.Core.Enums;

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

				emailAction.Body = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Email Confirmation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
                        padding: 20px;
                    }}
                    .confirmation-container {{
                        background-color: #ffffff;
                        border: 2px solid #007bff; /* Blue border */
                        border-radius: 10px;
                        padding: 20px;
                        max-width: 600px;
                        margin: 0 auto;
                        text-align: center;
                    }}
                    .confirmation-message {{
                        font-size: 18px;
                        margin-bottom: 20px;
                        color: #333333; /* Dark text color */
                    }}
                    .confirmation-link {{
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #007bff; /* Blue background */
                        color: #ffffff; /* White text color */
                        text-decoration: none;
                        border-radius: 5px;
                        transition: background-color 0.3s ease;
                    }}
                    .confirmation-link:hover {{
                        background-color: #0056b3; /* Darker blue on hover */
                    }}
                </style>
            </head>
            <body>
                <div class='confirmation-container'>
                    <p class='confirmation-message'>Please confirm your email address by clicking the button below:</p>
                    <a class='confirmation-link' href='{emailAction.Url}'>Confirm Email</a>
                </div>
            </body>
            </html>
        ";

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
				{ "Text-part", "Please confirm your email address by clicking the link provided." },
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

        public async Task SendOrganizerEmail(string email)
        {
            try
            {

                var client = new MailjetClient(_emailOption.ApiKey, _emailOption.SecretKey);

                // Construct the HTML email body with the confirmation link
                var body = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Email Confirmation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
                        padding: 20px;
                    }}
                    .confirmation-container {{
                        background-color: #ffffff;
                        border: 2px solid #007bff; /* Blue border */
                        border-radius: 10px;
                        padding: 20px;
                        max-width: 600px;
                        margin: 0 auto;
                        text-align: center;
                    }}
                    .confirmation-message {{
                        font-size: 18px;
                        margin-bottom: 20px;
                        color: #333333; /* Dark text color */
                    }}
                    .confirmation-link {{
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #007bff; /* Blue background */
                        color: #ffffff; /* White text color */
                        text-decoration: none;
                        border-radius: 5px;
                        transition: background-color 0.3s ease;
                    }}
                    .confirmation-link:hover {{
                        background-color: #0056b3; /* Darker blue on hover */
                    }}
                </style>
            </head>
            <body>
                <div class='confirmation-container'>
                    <p class='confirmation-message'>Your account has been verified</p>
                </div>
            </body>
            </html>
        ";

                var request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }.Property(Send.Messages, new JArray {
            new JObject {
                { "FromEmail", "ritika.shrestha707@gmail.com" },
                { "FromName", "EventSpace" },
                { "Recipients", new JArray {
                    new JObject {
                        { "Email",email },
                        { "Name", email }
                    }
                }},
                { "Subject", "Event Verse Organizer Verification" },
                { "Text-part", "Please confirm your email address by clicking the link provided." },
                { "Html-part", body }
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

                bool isfreeTicket = ticketResponseDTO.TicketType == EventVerse.Core.Enums.EventType.Free;
				// Construct the HTML email body with the ticket details
				string emailBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Ticket Purchase Confirmation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #eeeeee;
                        padding: 20px;
                    }}
                    .ticket-container {{
                        background-color: white;
                        border: 1px solid #ccc;
                        border-radius: 10px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        padding: 20px;
                        max-width: 600px;
                        margin: 0 auto;
                    }}
                    .ticket-details {{
                        border-bottom: 2px solid darkred;
                        padding-bottom: 10px;
                        margin-bottom: 20px;
                    }}
                    .ticket-details h2 {{
                        font-size: 24px;
                        margin-bottom: 10px;
                        color: darkred;
                    }}
                    .ticket-details p {{
                        margin: 5px 0;
                        color: black;
                    }}
                    .thank-you {{
                        font-size: 18px;
                        color: black;
                    }}
                </style>
            </head>
            <body>
                <div class='ticket-container'>
                    <div class='ticket-details'>
                        <h2>Ticket Details</h2>
                        <p><strong>Quantity:</strong> {ticketResponseDTO.Qty}</p>
                         {(isfreeTicket? "<p><strong>Total Price:</strong> Free Ticket</p>" : $"<p><strong>Total Price:</strong> {ticketResponseDTO.TotalPrice}</p>")}
                        {( !String.IsNullOrEmpty(ticketResponseDTO.TierName) ?  $"<p><strong>Total Price:</strong> Free Ticket </p>" :"")}


                        <p><strong>Event Type:</strong> {ticketResponseDTO.TicketType}</p>
                        <p><strong>Venue:</strong> {ticketResponseDTO.Venue}</p>
                        <p><strong>Event Date:</strong> {ticketResponseDTO.Eventdate}</p>
                    </div>
                    <p class='thank-you'>Thank you for purchasing the tickets! We appreciate your support.</p>
                </div>
            </body>
            </html>
        ";

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

		public async Task SendQrCheck(string userEmail, string image)
		{
			try
			{

				var client = new MailjetClient(_emailOption.ApiKey, _emailOption.SecretKey);

				string emailBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Ticket Purchase Confirmation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #eeeeee;
                        padding: 20px;
                    }}
                    .ticket-container {{
                        background-color: white;
                        border: 1px solid #ccc;
                        border-radius: 10px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        padding: 20px;
                        max-width: 600px;
                        margin: 0 auto;
                    }}
                    .ticket-details {{
                        border-bottom: 2px solid darkred;
                        padding-bottom: 10px;
                        margin-bottom: 20px;
                    }}
                    .ticket-details h2 {{
                        font-size: 24px;
                        margin-bottom: 10px;
                        color: darkred;
                    }}
                    .ticket-details p {{
                        margin: 5px 0;
                        color: black;
                    }}
                    .thank-you {{
                        font-size: 18px;
                        color: black;
                    }}
                </style>
            </head>
            <body>
                <div class='ticket-container'>
                    <div class='ticket-details'>
                        <h2>Ticket Details</h2>
                          <h2>QR Code</h2>
            <img src='data:image/png;base64,{image}' alt='QR Code'>
                      
                    </div>
                    <p class='thank-you'>Thank you for purchasing the tickets! We appreciate your support.</p>
                </div>
            </body>
            </html>
        ";

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



