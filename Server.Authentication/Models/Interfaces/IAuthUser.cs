namespace Server.Authentication.Models.Interfaces
{
    /// <summary>
    ///  Interface for ApplicationUser class
    /// </summary>
    public interface IAuthUser
    {
        /// <summary>
        /// User request id from token
        /// </summary>
        int Id { get; }
    }
}