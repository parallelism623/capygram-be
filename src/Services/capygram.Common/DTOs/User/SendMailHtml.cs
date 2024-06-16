namespace capygram.Common.DTOs.User
{
    public class SendMailHtml
    {
        public SendMailHtml() { }
        public SendMailHtml(string emailReceive, string body, string title, string usernameReceive)
        {
            EmailReceive = emailReceive;
            Body = body;
            Title = title;
            UsernameReceive = usernameReceive;
        }
        public string EmailReceive { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public string UsernameReceive { get; set; }

    }
}
