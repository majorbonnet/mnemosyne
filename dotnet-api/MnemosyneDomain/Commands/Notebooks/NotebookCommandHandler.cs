using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class NotebookCommandHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authService;

        public NotebookCommandHandler(MnemosyneContext context, IAuthorizationHandler authService)
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

            _context.Notebooks.Add(newNotebook);
            await _context.SaveChangesAsync();

            return new NotebookCreated(
                request.User,
                newNotebook.NotebookId,
                newNotebook.Created,
                newNotebook.Updated
            );
        }

        public async Task HandleAsync(DeleteNotebook request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            var notebook = await _context.Notebooks.FirstOrDefaultAsync(n => n.NotebookId == request.NotebookId);

            if (notebook is null) return;  

            _context.Notebooks.Remove(notebook);
            await _context.SaveChangesAsync();
        }
    }
}
