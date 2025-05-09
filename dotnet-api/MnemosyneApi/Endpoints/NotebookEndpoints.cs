using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Events;
using MnemosyneDomain.Queries.Notebooks;
using Wolverine;
using Wolverine.Http;

namespace MnemosyneApi.Endpoints
{
    public static class NotebookEndpoints
    {
        /// <summary>
        /// Get the notebooks for the current user
        /// </summary>
        /// <param name="bus"></param>
        /// <returns>A list of <see cref="Notebook"> instances</returns>
        [WolverineGet("/api/notebooks")]
        public static async Task<List<Notebook>> GetNotebooks(IMessageBus bus, [NotBody] User user, CancellationToken cancellationToken)
        {
            List<Notebook> notebooks = await bus.InvokeAsync<List<Notebook>>(new GetNotebooks(user), cancellationToken);

            if (notebooks.Count == 0)
            {
                NotebookCreated created = await bus.InvokeAsync<NotebookCreated>(new CreateNotebook(user), cancellationToken);

                notebooks = await bus.InvokeAsync<List<Notebook>>(new GetNotebooks(user), cancellationToken);
            }

            return notebooks;
        }

        /// <summary>
        /// Create a new notebook for the current user
        /// </summary>
        /// <param name="bus"></param>
        /// <returns>A <see cref="Notebook"/> instance with the new notebook info</returns>
        [WolverinePost("/api/notebooks")]
        public static async Task<Notebook> CreateNotebook(IMessageBus bus, [NotBody] User user, CancellationToken cancellationToken)
        {
            NotebookCreated result = await bus.InvokeAsync<NotebookCreated>(new CreateNotebook(user), cancellationToken);
            Notebook notebook = await bus.InvokeAsync<Notebook>(new GetNotebook(user, result.NotebookId), cancellationToken);

            return notebook;
        }
    }
}
