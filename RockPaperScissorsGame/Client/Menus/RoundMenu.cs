using Client.Menus.Interfaces;
using Client.Models;
using Client.Models.Interfaces;

namespace Client.Menus
{
    internal class RoundMenu : IRoundMenu
    {
        private readonly IRoom _room;
        private readonly IAccount _playerAccount;
        public RoundMenu(
            IRoom room,
            IAccount playerAccount)
        {
            _room = room;
            _playerAccount = playerAccount;
        }
    }
}