using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace LetsVotes.Client.Services.Toast;
public class ToastService() : IToastService
{
    ConcurrentQueue<ToastEventArgs> _queue = new();

    public event EventHandler<ToastEventArgs> ToastEvent = default!;

    bool _isToasting = false;

    public void Success(string message)
    {

        ToastEventArgs args = new(message, "toast-success");
        AddToQ(args);
    }

    public void Error(string message)
    {
        ToastEventArgs args = new(message, "toast-error");
        AddToQ(args);
    }


    public void Warning(string message)
    {
        ToastEventArgs args = new(message, "toast-warning");
        AddToQ(args);
    }

    public void Info(string message)
    {
        ToastEventArgs args = new(message, "toast-info");
        AddToQ(args);
    }

    private void AddToQ(ToastEventArgs args)
    {
        _queue.Enqueue(args);
        if (!_isToasting)
            _ = ProcessToastEventsAsync();

    }

    protected virtual Task RaiseToastEvent(ToastEventArgs args)
    {
        return Task.Run(() => ToastEvent?.Invoke(this, args));
    }

    private async Task ProcessToastEventsAsync()
    {
        _isToasting = true;
        while (_queue.Count > 0)
        {
            if (_queue.TryDequeue(out var args))
            {
                await RaiseToastEvent(args);
            }
        }
        _isToasting = false;
    }

}

[Flags]
public enum ToastPosition
{
    None = 0,
    Top = 1,
    Bottom = 2,
    Left = 4,
    Right = 8,
}

public static class ToastExtensions
{
    public static string ToString(this ToastPosition position)
    {
        return position.ToString().Replace(",", "");
    }
}

public class ToastEventArgs(string message = "", string cssClass = "") : EventArgs
{
    public string Message { get; set; } = message;

    // TODO upgrade to Enum
    public string CssClass { get; set; } = cssClass;
}