using System;

namespace Server.Authentication
{
    public static class HashingBase64
    {
        public static string DecodeBase64(this string encodedData)
        {
            var encodedDataAsBytes
                = Convert.FromBase64String(encodedData);
            return
                System.Text.Encoding.ASCII.GetString(encodedDataAsBytes);
        }

        public static string EncodeBase64(this string decodedData)
        {
            var dataArray = System.Text.Encoding.ASCII.GetBytes(decodedData);

            return Convert.ToBase64String(dataArray);
        }

        public static bool IsHashEqual(this string base64Data, string initialData)
        {
            var baseDecoded = EncodeBase64(initialData);
            
            return base64Data.Equals(baseDecoded, StringComparison.OrdinalIgnoreCase);
        }
    }
}