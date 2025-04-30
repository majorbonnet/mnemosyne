using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class NotebookQueryHandler
    {
        private readonly INotebookRepository _repository;

        public NotebookQueryHandler(INotebookRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Notebook>> HandleAsync(GetNotebooks request)
        {
            List<Notebook> notebooks = (await _repository.GetNotebooksByUserIdAsync(request.User.UserId))
                .Select(x => new Notebook(
                    x.NotebookId,
                    x.Created,
                    x.Updated,
                    x.Title
                ))
                .ToList();

            return notebooks;
        }
    }
}
