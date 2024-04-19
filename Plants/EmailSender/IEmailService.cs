namespace Plants.EmailSender
{
    public interface IEmailService
    {
        public Task SendEmail(Message message);
    }
}
