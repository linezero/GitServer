using System.ComponentModel.DataAnnotations;

namespace GitServer.Models
{
	public class LoginModel
    {
		[Required]
		public string Username { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
    }
}
