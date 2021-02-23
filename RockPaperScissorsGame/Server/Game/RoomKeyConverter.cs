using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Game.Models;

namespace Server.Game
{
    class RoomKeyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(RoomKey));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var roomKey = new RoomKey((string)jObject["SessionId"],(string)jObject["Login"]);
            return roomKey;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObject = new JObject
            {
                {
                    (
                        (string) value.GetType().GetProperty("SessionId")?.GetValue(value, null))!,
                    (string) value.GetType().GetProperty("Login")?.GetValue(value, null)!
                }
            };
            jObject.WriteTo(writer);
        }
    }
}