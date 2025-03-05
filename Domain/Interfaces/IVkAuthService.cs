using VkPostAnalyzer.Domain.Errors;

namespace VkPostAnalyzer.Domain.Interfaces
{
	public interface IVkAuthService
	{
		Task<Result<string>> GenerateAuthUrl();
		Task<Result<string>> HandleAuthResponse(string code, string deviceId, string state);
	}
}
