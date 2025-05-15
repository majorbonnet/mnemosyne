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
        [WolverinePost("/api/notebooks/{notebookId}/pages")]
        public static async Task<Page> CreatePage(
            IMessageBus bus,
            [NotBody] User user,
            Guid notebookId,
            CancellationToken cancellationToken)
        {
            PageCreated pageCreated = await bus.InvokeAsync<PageCreated>(new CreatePage(user, notebookId), cancellationToken);

            return await bus.InvokeAsync<Page>(new GetPage(user, notebookId, pageCreated.PageId), cancellationToken);
        }

        public class UpdatePageRequest
        {
            public string Contents { get; set; } = string.Empty;
        }

        [WolverinePost("/api/notebooks/{notebookId}/pages/{pageId}")]
        public static async Task UpdatePage(
            IMessageBus bus,
            [NotBody] User user,
            Guid notebookId,
            Guid pageId,
            UpdatePageRequest request,
            CancellationToken cancellationToken)
        {
            await bus.InvokeAsync(new UpdatePage(user, notebookId, pageId, request.Contents), cancellationToken);
        }

        [WolverineGet("/api/pages")]
        public static async Task<List<Page>> SearchPages(
            IMessageBus bus,
            [NotBody] User user,
            string query,
            CancellationToken cancellationToken)
        {
            // might as well short-circuit this here
            if (string.IsNullOrEmpty(query))
            {
                return new List<Page>();
            }

            return await bus.InvokeAsync<List<Page>>(new SearchPages(user, query), cancellationToken);
        }
    }
}
