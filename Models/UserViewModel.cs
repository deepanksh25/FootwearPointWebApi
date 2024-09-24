using System.ComponentModel.DataAnnotations;

namespace FootwearPointWebApi.Models
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "UserID")]

        public int UserID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Please do not enter values over 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Please do not enter values over 50 characters")]

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Role Id")]
        public int RoleID { get; set; }
        [Required]
        [MaxLength(11)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Hashed Password")]
        public string HashedPassword { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Created At")]

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public string CreatedAt { get; set; }
        [Display(Name = "Modified At")]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public string ModifiedAt { get; set; }

       
    }
}
