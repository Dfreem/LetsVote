﻿using LetsVote.Shared.DTO;

using Microsoft.AspNetCore.SignalR;

namespace LetsVote.Hubs;

public class TugHub : Hub<ITugClient>
{
    // From Server
    public async Task SendGameStateToAll(TugGameEventArgs args)
    {
        await Clients.Group("listeners").RecieveGameState(args);
    }

    public async Task AddToListenerGroup()
    {
        // TODO Use unique group name for individual games.
        await Groups.AddToGroupAsync(Context.ConnectionId, "listeners");
    }

    public async Task AddServer()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "server");
    }

    public async Task SendGameOver(GameOverArgs args)
    {
        await Clients.Group("listeners").GameOver(args);
    }

    // From Client
    public async Task TugRight()
    {
        await Clients.Group("server").Right();
    }

    public async Task TugLeft()
    {
        await Clients.Group("server").Left();
    }

    public async Task ConfigureGame(GameConfiguration config)
    {
        await Clients.Group("server").Configure(config);
    }
}

 public interface ITugClient
{
    // On Client
    Task RecieveGameState(TugGameEventArgs args);

    Task GameOver(GameOverArgs args);


    // On Server
    Task Right();

    Task Left();

    Task Configure(GameConfiguration config);
}
