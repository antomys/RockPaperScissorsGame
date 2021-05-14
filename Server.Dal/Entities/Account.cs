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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; init; }
        
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
        public string StatisticsId { get; set; }
        
        /// <summary>
        /// Linked to this player statistics
        /// </summary>
        [ForeignKey("StatisticsId")]
        public virtual Statistics Statistics { get; set; }
        
        public int RoomPlayerId { get; set; }
        
        [ForeignKey("RoomPlayerId")]
        public virtual RoomPlayers RoomPlayers { get; set; }
    }
}