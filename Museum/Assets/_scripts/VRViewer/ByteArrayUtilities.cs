using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;

/// <summary>
/// 
/// </summary>
class ByteArrayUtilities
{

    /// <summary>
    /// 
    /// </summary>
    public static void TestCompression()
    {
        byte[] test = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        byte[] result = Decompress(Compress(test));

        // This will fail, result.Length is 0
        Debug.Assert(result.Length == test.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Compress(byte[] data)
    {
        using (var compressedStream = new MemoryStream())
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Decompress(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        using (var resultStream = new MemoryStream())
        {
            var buffer = new byte[4096];
            int read;

            while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                resultStream.Write(buffer, 0, read);
            }

            return resultStream.ToArray();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ba"></param>
    /// <returns></returns>
    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2} ", b);
        return hex.ToString();
    }
}

