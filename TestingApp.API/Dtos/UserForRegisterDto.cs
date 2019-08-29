using System.ComponentModel.DataAnnotations;

namespace TestingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="password length should be between 4 and 8")]
        public string Password { get; set; }
    }
}