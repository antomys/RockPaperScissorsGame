using System;
using System.Collections.Generic;
using System.Text;

namespace RockPaperScissors.Models.Interfaces
{
    interface IAccount
    {
        Guid Id { get; }
        string Login { get; }
        string Email { get; }
        string Password { get; }
    }
}
