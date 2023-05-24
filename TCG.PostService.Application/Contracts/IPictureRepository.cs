using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts
{
    public interface IPictureRepository : IRepository<Domain.SalePicturePost>
    {
        Task<IEnumerable<Domain.SalePicturePost>> GetAllSalePicturePostPublicAsync(CancellationToken cancellationToken);
    }
}
