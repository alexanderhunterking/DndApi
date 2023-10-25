using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DnDTeamGame.Data.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        [Key]
        public int UserId { get; set; }
        
        [MaxLength(50)]
        public string? FirstName {get; set;}

        [MaxLength(50)]
        public string? LastName { get; set; }
        
        [Required]
        public DateTime DateCreated { get; set; }

        public virtual List<GameEntity> Games {get; set;} = new List<GameEntity>();

        public List<CharacterEntity> Characters { get; set; } = new List<CharacterEntity>();
    }
}
