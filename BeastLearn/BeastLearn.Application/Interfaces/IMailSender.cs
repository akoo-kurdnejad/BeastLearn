namespace BeastLearn.Application.Interfaces
{
    public interface IMailSender
    {
        void Send(string to, string subject, string body);
    }
}