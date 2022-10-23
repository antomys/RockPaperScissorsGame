namespace Server.Authentication;

/// <summary>
///     Default hashing base64 methods.
/// </summary>
public static class HashingBase64
{
    /// <summary>
    ///     Decodes input string from BASE64 to a regular string.
    /// </summary>
    /// <param name="encodedData">Encoded in BASE64 payload.</param>
    /// <returns>Decoded string.</returns>
    public static string DecodeBase64(this string encodedData)
    {
        var encodedDataAsBytes
            = Convert.FromBase64String(encodedData);

        return
            System.Text.Encoding.ASCII.GetString(encodedDataAsBytes);
    }

    /// <summary>
    ///     Encodes input string to BASE64 From a regular string.
    /// </summary>
    /// <param name="initialData">Payload to be encoded.</param>
    /// <returns>Encoded string.</returns>
    public static string EncodeBase64(this string initialData)
    {
        var dataArray = System.Text.Encoding.ASCII.GetBytes(initialData);

        return Convert.ToBase64String(dataArray);
    }

    /// <summary>
    ///     Compares initial encoded data with base64 data.
    /// </summary>
    /// <param name="base64Data">Encoded data.</param>
    /// <param name="initialData">Initial payload.</param>
    /// <returns>
    /// <para>
    ///     <c>true</c> - when initial data, encoded in base64 is identical with base64 data.
    /// </para>
    ///     <c>false</c> - otherwise.
    /// </returns>
    public static bool IsHashEqual(this string base64Data, string initialData)
    {
        var baseDecoded = EncodeBase64(initialData);

        return base64Data.Equals(baseDecoded, StringComparison.OrdinalIgnoreCase);
    }
}