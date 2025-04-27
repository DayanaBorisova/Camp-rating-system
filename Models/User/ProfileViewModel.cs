using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Camp_Rating_System.Models.User
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "The First Name field is required.")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]+$", ErrorMessage = "The First Name field can only contain letters, digits, and spaces.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s]+$", ErrorMessage = "The Last Name field can only contain letters, digits, and spaces.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public required string Email { get; set; }
    }
}
