using capygram.Common.Abstraction;

namespace capygram.Auth.DependencyInjection.Options
{
    public class MasstransitOptions
    {
        public string VHost { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }    
        public string ExchangeType { get; set; }
        public string SmsQueueName { get; set; }
        public string EmailQueueName { get; set; }

       
    }
}

/*
    "Host": "localhost",
    "VHost": "capygram",
    "UserName": "guest",
    "Password": "guest",
    "ExchangeName": "send-notification-exchange",
    "ExchangeType": "topic",
    "SmsQueueName": "sms-queue",
    "EmailQueueName": "email-queue"
 */