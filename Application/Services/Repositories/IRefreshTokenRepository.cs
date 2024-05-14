using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Application.Services.Repositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken, int>, IRepository<RefreshToken, int>
{
    // Kullanıcının eski refresh token'larına ulaşmak için kullanılan metot.
    Task<List<RefreshToken>> GetOldRefreshTokensAsync(int userID, int refreshTokenTTL);
}
