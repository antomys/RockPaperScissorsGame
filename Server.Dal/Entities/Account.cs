using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities
{
    [Table("Accounts")]
    public class Account
    {
        /// <summary>
        /// Id of account. Unique to everyone and similar with Statistics Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// Nick name of Account
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Password of the Account
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Statistics id, connected to this account
        /// </summary>
        public int StatisticsId { get; set; }
        
        /// <summary>
        /// Linked to this player statistics
        /// </summary>
        [ForeignKey("StatisticsId")]
        public virtual Statistics Statistics { get; set; }
        
        public virtual ICollection<RoomPlayers> FirstPlayer { get; set; }
        public virtual ICollection<RoomPlayers> SecondPlayer { get; set; }
    }
}