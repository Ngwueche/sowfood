using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities.SowFoodLinkUp;

namespace SowFoodProject.Models.Entities
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<string>
    {
        [Required]
        [PersonalData]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        [StringLength(100)]
        public string LastName { get; set; }
        public string SowFoodCompanyId { get; set; }

        public string? Address { get; set; }
        public string ProfileImageUrl { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;
        public string? Gender { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime? LastLoginAt { get; set; }
        public string? PasswordResetToken { get; set; }  // Custom column for password reset
        public DateTime? PasswordResetExpiry { get; set; } // Expiry timestamp
        public string? RefreshToken { get; set; }
        public bool? PasswordResetVerified { get; set; } = false;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public virtual SowFoodCompany SowFoodCompany { get; set; }

    }
}