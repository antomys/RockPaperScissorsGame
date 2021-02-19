using System;
using System.Collections.Generic;
using System.Text;

namespace RockPaperScissors.Models.Interfaces
{
    internal interface IAccountDto
    {
        //Guid Id { get; }
        string Login { get; }
        //string Email { get; }
        string Password { get; }
    }
}
