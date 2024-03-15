using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "New Password is required.")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm New Password Is Required.")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Password doesn't match")]
		public string ConfirmNewPassword { get; set; }
	}
}
