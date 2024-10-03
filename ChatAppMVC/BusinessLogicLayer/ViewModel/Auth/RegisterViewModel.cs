namespace ChatAppMVC.ViewModel.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "UserName is required.")]
        [StringLength(50, ErrorMessage = "UserName cannot be longer than 50 characters.")]
        public string UserName { get; set; }

		
		public IFormFile? ProfilePicture { get; set; }

        [Required(ErrorMessage ="Password is required")]
		[DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
