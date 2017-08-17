
namespace GitServer.Services
{
	/// <summary>
	/// Static class for handling git authentication
	/// </summary>
	public static class GitAuthenticationService
    {
		/// <summary>
		/// Checks if credentials are valid.
		/// </summary>
		/// <param name="username">The username</param>
		/// <param name="password">The corresponding password</param>
		/// <returns><c>true</c> if authentication was successful, otherwise <c>false</c>.</returns>
		public static bool CheckCredentials(string username, string password)
		{
			//HACK: Reading the username and password from a database is perhaps a better idea :P
			if (username == "robin")
				return true;
			return false;
		}
    }
}
