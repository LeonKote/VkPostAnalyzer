using Domain.Models;

namespace Domain.Interfaces.Repositories
{
	public interface IAuthRequestRepository
	{
		Task AddAsync(AuthRequest authRequest);
		Task<AuthRequest?> GetByStateAsync(string state);
		Task RemoveAsync(AuthRequest authRequest);
	}
}
