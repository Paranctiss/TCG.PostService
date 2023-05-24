using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories
{
    public class PictureRepository : Repository<SalePicturePost>, IPictureRepository
    { 

        protected readonly ServiceDbContext _dbContext;
        public PictureRepository(ServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<SalePicturePost>> GetAllSalePicturePostPublicAsync(CancellationToken cancellationToken)
        {
           return await _dbContext.SalePicturePosts.ToListAsync();
        }

    }
}
