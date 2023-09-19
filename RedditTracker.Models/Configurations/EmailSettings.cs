using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public class EmailSettings: IEmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string SmtpUser { get; set; }
        public string Password { get; set; }
        public string DevelopmentRecipient { get; set; }
    }
}
