﻿using Domain.Errors;

namespace Domain.Interfaces
{
	public interface IVkAuthService
	{
		Task<Result<string>> GenerateAuthUrl();
		Task<Result<string>> HandleAuthResponse(string code, string deviceId, string state);
	}
}
