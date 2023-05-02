using MassTransit.Initializers.Variables;
using Microsoft.EntityFrameworkCore;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories;

public class GradingRepository : Repository<Grading>, IGradingRepository
{
    protected readonly ServiceDbContext _dbContext;
    public GradingRepository(ServiceDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Grading> GetDetachedByIdAsync(int id, CancellationToken cancellationToken)
    {
        Grading grading = await GetByIdAsync(id, cancellationToken);

        if (grading != null)
        {
            // Detach the entity from the DbContext
            _dbContext.Entry(grading).State = EntityState.Detached;
        }



        return grading;
    }
}