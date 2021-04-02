using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [Table("Sessions")]
    public class ActiveSessions
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SessionId { get; set; }

        public string AccountId { get; set; }
        
        public ActiveSessions()
        {
            
        }

        public ActiveSessions(string sessionId)
        {
            SessionId = sessionId;
        }

        public ActiveSessions(string accountId, string sessionId)
        {
            SessionId = sessionId;
            AccountId = accountId;
        }

        
    }
}