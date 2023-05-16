using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Contracts;

    public interface IGradingRepository : IRepository<Domain.Grading>
    {
    Task<Grading> GetDetachedByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Domain.Grading>> GetAllGradingAsync(CancellationToken cancellationToken);
    }


