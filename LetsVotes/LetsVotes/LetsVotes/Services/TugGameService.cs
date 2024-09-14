
using LetsVote.Shared.DTO;

using LetsVote.Hubs;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace LetsVote.Services;

/// <summary>
/// A hosted background service used for running a Tug-o-war game. Inherits from an <see cref="ITugClient"/> but only impliments the hub methods it listens for.
/// </summary>
public class TugGameService : IHostedService, ITugClient
{
    HubConnection _hubConnection;

    int _teamA;

    int _teamB;

    GameConfiguration _config = new();

    public TugGameService()
    {
        // TODO add IOption singleton for this
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5255/tug-game")
            .WithAutomaticReconnect()
            .WithStatefulReconnect()
            .Build();

        // Register listener callbacks
        _hubConnection.On("Left", Left);
        _hubConnection.On("Right", Right);

    }

    // ==================== Hosted Service Methods ====================
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        // retry connection until server is running.
        while (true)
        {
            try
            {
                Console.WriteLine("connecting to host");
                await _hubConnection.StartAsync(cancellationToken);
                break;
            }
            catch
            {
                Console.WriteLine("retrying connection");
                await Task.Delay(1000, cancellationToken);
            }

        }
        await _hubConnection.InvokeAsync("AddServer", cancellationToken: cancellationToken);

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _hubConnection.StopAsync(cancellationToken);
        await _hubConnection.DisposeAsync();
    }


    // ==================== TugClient Methods ======================
    public async Task Left()
    {
        _teamA++;
        TugGameEventArgs args = new()
        {
            TeamA = _teamA,
            TeamB = _teamB
        };
        await _hubConnection.InvokeAsync<TugGameEventArgs>("SendGameStateToAll", args);
        if (_teamA >= _config.VoteTarget)
        {
            GameOverArgs gameOverArgs = new()
            {
                TeamA = _teamA,
                TeamB = _teamB,
                WinningTeam = "A"
            };
            await _hubConnection.InvokeAsync<GameOverArgs>("SendGameOver", gameOverArgs);
        }
    }
    public async Task Right()
    {
        _teamB++;
        TugGameEventArgs args = new()
        {
            TeamA = _teamA,
            TeamB = _teamB
        };
        await _hubConnection.InvokeAsync<TugGameEventArgs>("SendGameStateToAll", args);
        if (_teamB >= _config.VoteTarget)
        {
            GameOverArgs gameOverArgs = new()
            {
                TeamA = _teamA,
                TeamB = _teamB,
                WinningTeam = "B"
            };
            await _hubConnection.InvokeAsync<GameOverArgs>("SendGameOver", gameOverArgs);
        }
    }

    public Task Configure(GameConfiguration config)
    {
        _config = config;
        return Task.CompletedTask;
    }

    // No-Op
    // These are called by this service, so we don't listen for them.
    public Task RecieveGameState(TugGameEventArgs args)
    {
        throw new NotImplementedException();
    }

    public Task<string> RecieveServerConnectionId(string connectionId)
    {
        throw new NotImplementedException();
    }

    public Task GameOver(GameOverArgs args)
    {
        throw new NotImplementedException();
    }

}
