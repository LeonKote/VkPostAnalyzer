namespace Domain.Models
{
	public class AuthRequest
	{
		public long Id { get; set; }
		public string State { get; set; }
		public string CodeVerifier { get; set; }
		public DateTime CreateDate { get; set; }

		public AuthRequest(string state, string codeVerifier, DateTime createDate)
		{
			State = state;
			CodeVerifier = codeVerifier;
			CreateDate = createDate;
		}
	}
}
