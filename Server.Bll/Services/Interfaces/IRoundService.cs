using System.Threading.Tasks;
using Server.Bll.Models;
using OneOf;
using Server.Bll.Exceptions;

namespace Server.Bll.Services.Interfaces;

public interface IRoundService
{
    Task<OneOf<RoundModel, CustomException>> CreateAsync(string userId, string roomId);
    
    Task<RoundModel> MakeMoveAsync();
    
    Task<OneOf<RoundModel, CustomException>> UpdateAsync(string userId, RoundModel roundModel);
}