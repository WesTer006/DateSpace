using DataAccessLayer.Entities;


namespace BusinessLogicLayer.Interfaces
{
	public interface ITokenService
	{
		string GenerateJwtToken(AppUser user);
		string GenerateRefreshToken();
	}
}
