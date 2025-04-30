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
        private readonly IRepository<Models.Notebook> _repository;

        public NotebookQueryHandler(IRepository<Models.Notebook> repository)
        {
            _repository = repository;
        }

        public List<Notebook> Handle(GetNotebooks request)
        {
            List<Notebook> notebooks = _repository.Find(n => n.UserId == request.User.UserId)
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
