using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Events;
using Wolverine;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class NotebookQueryHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authorizationHandler;

        public NotebookQueryHandler(MnemosyneContext context, IAuthorizationHandler authorizationHandler)
        {
            _context = context;
            _authorizationHandler = authorizationHandler;
        }

        public async Task<List<Notebook>> HandleAsync(GetNotebooks request, CancellationToken cancellationToken = default)
        {
            List<Notebook> notebooks = await _context.Notebooks
                .Where(n => n.UserId == request.User.UserId)
                .Select(x => new Notebook(
                    x.NotebookId,
                    x.Created,
                    x.Updated,
                    x.Title
                ))
                .ToListAsync(cancellationToken);

            return notebooks;
        }

        public async Task<Notebook?> HandleAsync(GetNotebook request, CancellationToken cancellationToken = default)
        {
            if (!(await _authorizationHandler.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner))) return null;

            if (await _context.Notebooks.FindAsync(request.NotebookId, cancellationToken) is Models.Notebook notebook)
            {
                return new Notebook(
                    notebook.NotebookId,
                    notebook.Created,
                    notebook.Updated,
                    notebook.Title
                );
            }

            return null;      
        }
    }
}
