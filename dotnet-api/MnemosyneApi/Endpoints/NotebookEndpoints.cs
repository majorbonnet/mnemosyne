using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Notebooks;
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
        public static async Task<List<Notebook>> GetNotebooks(IMessageBus bus, [NotBody] User user)
        {
            return await bus.InvokeAsync<List<Notebook>>(new GetNotebooks(user));
        }

        /// <summary>
        /// Create a new notebook for the current user
        /// </summary>
        /// <param name="bus"></param>
        /// <returns>A <see cref="NotebookCreated"/> instance with the new notebook info</returns>
        [WolverinePost("/api/notebooks")]
        public static async Task<NotebookCreated> CreateNotebook(IMessageBus bus, [NotBody] User user)
        {
            return await bus.InvokeAsync<NotebookCreated>(new CreateNotebook(user));
        }
    }
}
