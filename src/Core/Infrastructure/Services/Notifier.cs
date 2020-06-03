using Domain.AggregatesModel.InstrumentsAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class Notifier : INotifier
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly string _client;
        private readonly string _reciver;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Notifier> _logger;
        public Notifier(IConfiguration configuration, ILogger<Notifier> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _userName = _configuration["Smtp:UserName"];
            _password = _configuration["Smtp:Password"];
            _client = _configuration["Smtp:Client"];
            _reciver = _configuration["Smtp:Reciver"];
        }
        public void SendMail(string subject, string body, IEnumerable<Instrument> instrumentDomains)
        {
            try
            {
                var msg = string.Empty;
                foreach (var item in instrumentDomains)
                {
                    msg += $"<br>Symbol: <b>{item.Symbol}</b> isin {item.Isin}.<br>";
                }
                SendMail(subject, body + msg);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Notifier");
            }
        }
        public void SendMail(string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(_userName);
                message.To.Add(_reciver);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                SmtpClient smtpClient = new SmtpClient(_client, 587);
                smtpClient.UseDefaultCredentials = true;

                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_userName, _password);
                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Notifier");
            }
        }
    }
}
