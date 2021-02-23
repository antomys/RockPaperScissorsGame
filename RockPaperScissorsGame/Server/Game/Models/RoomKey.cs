using System.ComponentModel;
using Newtonsoft.Json;

namespace Server.Game.Models
{
    [JsonConverter(typeof(RoomKeyConverter))]
    public class RoomKey
    {
        [JsonProperty("SessionId")] public string SessionId { get; set; }
        [JsonProperty("Login")] public string Login { get; set; }

        public RoomKey(string sessionId, string login)
        {
            SessionId = sessionId;
            Login = login;
        }
        
        /*
        public override int GetHashCode()
        {
            return SessionId.GetHashCode() ^ Login.GetHashCode();
        }

        private bool Equals(RoomKey roomKey)
        {
            return roomKey.Login.Equals(Login) && roomKey.SessionId.Equals(SessionId);
        }

        public override bool Equals(object obj)
        {
            return obj != null && Equals((RoomKey) obj);
        }

        public static bool operator ==(RoomKey left, RoomKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RoomKey left, RoomKey right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return serialized;
        }*/
    }
}
    