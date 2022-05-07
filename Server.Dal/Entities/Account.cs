using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Dal.Entities;

[Table("Accounts")]
public class Account
{
    /// <summary>
    ///     Id of account. Unique to everyone and similar with Statistics Id
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
        
    /// <summary>
    ///     Nick name of Account.
    /// </summary>
    public string Login { get; set; }
        
    /// <summary>
    /// Password of the Account
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    ///     Linked to this player statistics
    /// </summary>
    public virtual Statistics Statistics { get; set; }
}