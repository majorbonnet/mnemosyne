using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class NotebookCommandHandler
    {
        private readonly MnemosyneContext _context;
        private readonly AuthorizationHandler _authService;
        public NotebookCommandHandler(MnemosyneContext context, AuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<NotebookCreated> HandleAsync(CreateNotebook request)
        {
            Notebook newNotebook = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = request.User.UserId
            };

            NotebookPage defaultPage = new()
            {
                Notebook = newNotebook,
                NotebookPageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            await _context.Notebooks.AddAsync(newNotebook);
            await _context.NotebookPages.AddAsync(defaultPage);

            await _context.SaveChangesAsync();

            return new NotebookCreated(
                newNotebook.NotebookId,
                newNotebook.Created,
                newNotebook.Updated
            );
        }

        public async Task HandleAsync(DeleteNotebook request)
        {
            Notebook? notebook = await _context.Notebooks.FindAsync(request.NotebookId);

            if (notebook is not null)
            {
                if (!_authService.IsAuthorized(request.User, notebook.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

                _context.Notebooks.Remove(notebook);
                await _context.SaveChangesAsync();
            } 
        }
    }
}
