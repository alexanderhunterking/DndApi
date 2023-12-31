using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace DnDTeamGame.Data.Entities
{
    
    public class AbilityEntity
    {
        [Key]
        public int AbilityId { get; set; }

        [Required, MinLength(4), MaxLength(50)]
        public string AbilityName { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(7500)]
        public string AbilityDescription { get; set; } = string.Empty;

        //* State if the ability is going to be an Attack, Healing, Defensive, 
        [Required]
        public string AbilityEffectType { get; set; }

        //* Determines what the effect of the Ability is going to have
        [Required]
        public bool AbilityEffectAttack { get; set; }
        [Required]
        public bool AbilityEffectHealthEnhancement { get; set; }
        [Required]
        public bool AbilityEffectDefenseEnhancement { get; set; }
        [Required]
        public bool AbilityHasStatusEffect { get; set; }

        //*Determines if the ability is going to affect one or more enemies if it is an attack type Ability
        [Required]
        public bool AbilityDamageSingleEnemy { get; set; }
        [Required]
        public bool AbilityDamageMultipleEnemy { get; set; }

        //* These Determine how much of something the Ability does or How long it may last.
        [Required, Range(0, 40)]
        public int AbilityAttackDamage { get; set; }

        [Required, Range(0, 100)]
        public int AbilityHealingAmount { get; set; }

        [Required, Range(0, 100)]
        public int AbilityDefenseIncrease { get; set; }

        [Range(0, 180)]
        public string? AbilityEffectTimeLimit { get; set; } = string.Empty;

        //todo Helps to setup a many to many foreign key relationship with the
        //todo Characters Table and creates a join table containing the CharacterId and 
        //todo the ability ids assigned to the character
        public ICollection<CharacterEntity> CharacterList { get; set; }

        public AbilityEntity()
        {
            CharacterList = new HashSet<CharacterEntity>();
        }
    }
}