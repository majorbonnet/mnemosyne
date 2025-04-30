using Microsoft.AspNetCore.Mvc;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Events;
using MnemosyneDomain.Queries.NotebookPages;
using Wolverine;
using Wolverine.Http;

namespace MnemosyneApi.Endpoints
{
    public static class NotebookPageEndpoints
    {
        /// <summary>
        /// Get the pages for a single notebook
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="request"></param>
        /// <returns>A list of <see cref="NotebookPage"/> instances</returns>
        [WolverineGet("/api/notebooks/{notebookId}")]
        public static async Task<List<NotebookPage>> GetNotebookPages(
            IMessageBus bus,
            [NotBody] User user,
            int notebookId)
        {
            return await bus.InvokeAsync<List<NotebookPage>>(new GetNotebookPages(user, notebookId));
        }

        /// <summary>
        /// Create a new page in a notebook
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="request"></param>
        /// <returns>A <see cref="NotebookPageCreated"/> instance with the new page info</returns>
        [WolverinePost("/api/notebooks/{notebookId}")]
        public static async Task<NotebookPageCreated> CreateNotebookPage(
            IMessageBus bus,
            [NotBody] User user,
            int notebookId)
        {
            return await bus.InvokeAsync<NotebookPageCreated>(new CreateNotebookPage(user, notebookId));
        }

        public class UpdateNotebookPageRequest
        {
            public string? Title { get; set; }
            public string? Contents { get; set; }
        }

        [WolverinePost("/api/notebooks/{notebookId}/{notebookPageId}")]
        public static async Task UpdateNotebookPage(
            IMessageBus bus,
            [NotBody] User user,
            int notebookId,
            Guid notebookPageId,
            UpdateNotebookPageRequest request)
        {
            if (request.Contents is null && request.Title is null) return;

            await bus.InvokeAsync(new UpdateNotebookPage(user, notebookId, notebookPageId, request.Title, request.Contents));
        }
    }
}
