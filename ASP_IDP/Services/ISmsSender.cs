namespace ASP_IDP.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
