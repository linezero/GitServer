using System.ComponentModel.DataAnnotations;

namespace GitServer.Models
{
	public class RegisterModel
    {
		[Required]
		[MinLength(4)]
		[MaxLength(25)]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[MinLength(8)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[MinLength(8)]
		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		public string ConfirmPassword { get; set; }
    }
}
