using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Contexts;

namespace Persistance.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, int, BaseDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDbContext context)
        : base(context) { }

    // UserId'ye göre OperationClaim'lerini getirmesini sağlayan bir metot.
    public async Task<IList<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId)
    {
        var operationClaims = await Query()
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => new OperationClaim { Id = p.OperationClaimId, Name = p.OperationClaim.Name })
            .ToListAsync();
        return operationClaims;
    }
}
