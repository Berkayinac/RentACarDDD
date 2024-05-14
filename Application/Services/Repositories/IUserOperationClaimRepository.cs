using Core.Persistence.Repositories;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories;

public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim, int>, IRepository<UserOperationClaim, int>
{
    // UserId'ye göre OperationClaim'lerini getirmesini sağlayan bir metot.
    Task<IList<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId);
}
