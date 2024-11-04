using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RR.DiceDuel.ExternalServices.EntityFramework.Entities;

[Index(nameof(UserName), IsUnique = true)]
public class UserEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
    
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}