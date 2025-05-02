using Microsoft.AspNetCore.Mvc;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Pages;
using MnemosyneDomain.Events;
using MnemosyneDomain.Queries.Pages;
using Wolverine;
using Wolverine.Http;

namespace MnemosyneApi.Endpoints
{
    public static class PageEndpoints
    {
        /// <summary>
        /// Get the pages for a single notebook
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="request"></param>
        /// <returns>A list of <see cref="Page"/> instances</returns>
        [WolverineGet("/api/notebooks/{notebookId}")]
        public static async Task<List<Page>> GetPages(
            IMessageBus bus,
            [NotBody] User user,
            Guid notebookId)
        {
            return await bus.InvokeAsync<List<Page>>(new GetPages(user, notebookId));
        }

        /// <summary>
        /// Create a new page in a notebook
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="request"></param>
        /// <returns>A <see cref="PageCreated"/> instance with the new page info</returns>
        [WolverinePost("/api/notebooks/{notebookId}")]
        public static async Task<PageCreated> CreatePage(
            IMessageBus bus,
            [NotBody] User user,
            Guid notebookId)
        {
            return await bus.InvokeAsync<PageCreated>(new CreatePage(user, notebookId));
        }

        public class UpdatePageRequest
        {
            public string Contents { get; set; } = string.Empty;
        }

        [WolverinePost("/api/notebooks/{notebookId}/{pageId}")]
        public static async Task UpdatePage(
            IMessageBus bus,
            [NotBody] User user,
            Guid notebookId,
            Guid pageId,
            UpdatePageRequest request)
        {
            await bus.InvokeAsync(new UpdatePage(user, notebookId, pageId, request.Contents));
        }
    }
}
