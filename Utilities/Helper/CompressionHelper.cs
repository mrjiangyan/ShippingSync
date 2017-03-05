using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Utilities.Helper
{
    public static class CompressionHelper
    {
        /// <summary>
        ///     Gzip解压函数
        /// </summary>
        /// <param name="data">解压缩之前的原始数据</param>
        /// <returns></returns>
        public static IEnumerable<byte> GzipDecompress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                using (var stream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    stream.Flush();
                    var nSize = 256*1024 + 256; //假设字符串不会超过256K
                    var decompressBuffer = new byte[nSize];
                    var nSizeIncept = stream.Read(decompressBuffer, 0, nSize);
                    stream.Close();
                    return decompressBuffer.Take(nSizeIncept); //转换为普通的字符串
                }
            }
        }

        /// <summary>
        ///     Gzip压缩函数
        /// </summary>
        /// <param name="data">需要压缩的数据</param>
        /// <returns></returns>
        public static byte[] DeflateByte(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var stream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
                var compressedData = ms.ToArray();
                ms.Close();
                return compressedData;
            }
        }
    }
}