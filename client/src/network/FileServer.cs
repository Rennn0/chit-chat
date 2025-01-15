using System.IO.Compression;
using System.Net.Sockets;
using System.Text;
using FileStream = System.IO.FileStream;

namespace client.network;

public class FileServer
{
    /// <summary>
    ///     fileserverze agzavnis fails da abrunebs chawerili bytebis raodenobas
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static async Task<int> SendFileAsync(string[] paths)
    {
        const string serverIp = "10.0.0.4";
        const int port = 8080;

        string tempZipFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");
        try
        {
            await using (FileStream zipStream = new FileStream(tempZipFile, FileMode.Create))
            {
                using (
                    ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, false)
                )
                {
                    foreach (string path in paths)
                    {
                        zipArchive.CreateEntryFromFile(path, Path.GetFileName(path));
                    }
                }
            }

            byte[] zipData = await File.ReadAllBytesAsync(tempZipFile);

            byte[] fileNameBytes = Encoding.UTF8.GetBytes(Path.GetFileName(tempZipFile));
            byte[] fileNameLengthBytes = BitConverter.GetBytes(
                System.Net.IPAddress.HostToNetworkOrder(fileNameBytes.Length)
            );

            byte[] payload = new byte[
                fileNameLengthBytes.Length + fileNameBytes.Length + zipData.Length
            ];
            Buffer.BlockCopy(fileNameLengthBytes, 0, payload, 0, fileNameLengthBytes.Length);
            Buffer.BlockCopy(
                fileNameBytes,
                0,
                payload,
                fileNameLengthBytes.Length,
                fileNameBytes.Length
            );
            Buffer.BlockCopy(
                zipData,
                0,
                payload,
                fileNameLengthBytes.Length + fileNameBytes.Length,
                zipData.Length
            );

            using TcpClient client = new TcpClient(serverIp, port);
            await using NetworkStream ns = client.GetStream();

            await ns.WriteAsync(payload, 0, payload.Length);
            return zipData.Length;
        }
        finally
        {
            if (File.Exists(tempZipFile))
            {
                File.Delete(tempZipFile);
            }
        }
    }
}
