using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public interface IEmailSettings
    {
        string DevelopmentRecipient { get; }
        string Password { get; }
        string Receiver { get; }
        string Sender { get; }
        string SenderName { get; }
        int SmtpPort { get; }
        string SmtpServer { get; }
        string SmtpUser { get; }
    }
}
