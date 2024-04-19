using MimeKit;

namespace Plants.EmailSender
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string contetn)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("Куколд Мафия", x)));
            Subject = subject;
            Content = contetn;
        }
    }
}
