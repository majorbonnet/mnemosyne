using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MnemosyneApi.Extensions;
using MnemosyneDomain.Events;
using MnemosyneDomain.Queries.Notebooks;
using Wolverine;

namespace MnemosyneApi.Hubs
{
    public class NotebookSyncHub : Hub
    {
        private static Dictionary<Guid, string> _connectedUsers = new Dictionary<Guid, string>();

        public static Dictionary<Guid, string> ConnectedUsers => _connectedUsers;

        public override Task OnConnectedAsync()
        {
            if (Context.User is not null && Context.User!.GetUserId() is Guid userId)
            {
                // Store the connection ID associated with the user ID
                _connectedUsers[userId] = Context.ConnectionId;                
            }

            // Logic to handle when a client connects to the hub
            return base.OnConnectedAsync();
        }
    }

    public class NotebookCreatedHandler
    {
        private readonly IHubContext<NotebookSyncHub> _hubContext;

        public NotebookCreatedHandler(IHubContext<NotebookSyncHub> hub)
        {
            _hubContext = hub;
        }

        public async Task HandleAsync(NotebookCreated notebookCreated, IMessageBus bus, CancellationToken cancellationToken)
        {
            if (NotebookSyncHub.ConnectedUsers.TryGetValue(notebookCreated.User.UserId, out var connectionId))
            {
                Notebook notebook = await bus.InvokeAsync<Notebook>(new GetNotebook(notebookCreated.User, notebookCreated.NotebookId), cancellationToken);

                await _hubContext.Clients.Client(connectionId).SendAsync("NotebookCreated", notebook);
            }
        }
    }
}
