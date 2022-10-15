using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities;

[Table(nameof(Account))]
public class Account
{
    /// <summary>
    ///     Id of account. Unique to everyone and similar with Statistics Id
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; init; }
        
    /// <summary>
    ///     Nick name of Account.
    /// </summary>
    public string Login { get; init; }
        
    /// <summary>
    /// Password of the Account
    /// </summary>
    public string Password { get; init; }
    
    /// <summary>
    ///     Linked to this player statistics
    /// </summary>
    public virtual Statistics Statistics { get; set; }
}