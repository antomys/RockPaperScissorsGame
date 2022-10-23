// using Client.Host.Extensions;
// using Client.Host.Models;
// using Client.Host.Services.RequestProcessor;
// using Client.Host.Services.RequestProcessor.RequestModels;
// using Client.Host.Services.RequestProcessor.RequestModels.Impl;
// using Newtonsoft.Json;
//
// namespace Client.Host.Services;
//
// public class RoomService : IRoomService
// {
//     private readonly TokenModel _tokenModel;
//     private readonly IRequestPerformer _requestPerformer;
//
//     public RoomService(TokenModel tokenModel, 
//         IRequestPerformer requestPerformer)
//     {
//         _tokenModel = tokenModel;
//         _requestPerformer = requestPerformer;
//     }
//     public async Task<Room> CreateRoom(bool isPrivate, bool isTraining)
//     {
//         Console.WriteLine("Trying to create a room.");
//             
//         var options = new RequestOptions
//         {
//             Address = $"room/create?isPrivate={isPrivate}",
//             IsValid = true,
//             Headers = new Dictionary<string, string>
//             {
//                 {
//                     "Authorization", _tokenModel.BearerToken
//                 },
//                 {
//                     "X-Training", isTraining.ToString()
//                 }
//             },
//             Method = RequestMethod.Post,
//             Body = string.Empty,
//             Name = "Create Room"
//         };
//         var reachedResponse = await _requestPerformer
//             .PerformRequestAsync(options);
//             
//         if (reachedResponse.TryParseJson<ErrorModel>(out var errorModel))
//         {
//             TextWrite.Print(errorModel.Message, ConsoleColor.Red);
//             return null;
//         }
//
//         var room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
//         return room;
//     }
// }
//
// public interface IRoomService
// {
//     Task<Room> CreateRoom(bool isPrivate, bool isTraining);
// }