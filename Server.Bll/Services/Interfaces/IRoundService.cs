using System.Threading.Tasks;
using Server.Bll.Models;
using OneOf;
using Server.Bll.Exceptions;

namespace Server.Bll.Services.Interfaces;

public interface IRoundService
{
    Task<OneOf<RoundModel, CustomException>> CreateAsync(int userId, int roomId);
    Task<RoundModel> MakeMoveAsync();
    Task<OneOf<RoundModel, CustomException>> UpdateAsync(int userId, RoundModel roundModel);
}