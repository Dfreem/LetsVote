namespace LetsVotes.Client.Services.Toast
{
    public interface IToastService
    {
        event EventHandler<ToastEventArgs> ToastEvent;

        void Error(string message);
        void Info(string message);
        void Success(string message);
        void Warning(string message);
    }
}