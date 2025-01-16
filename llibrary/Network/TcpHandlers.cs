using System.IO.Compression;
using System.Net.Sockets;
using System.Text;

namespace llibrary.Network;

public class TcpHandlers
{
    private const string _serverIp = "10.0.0.4";
    private const int _port = 8080;

    /// <summary>
    ///     fileserverze agzavnis fails
    /// </summary>
    /// <param name="paths">failebis absoluturi misamarti</param>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <returns>sheqmnili zip guid</returns>
    public static async Task<string> SendFileAsync(
        string[] paths,
        string ip = _serverIp,
        int port = _port
    )
    {
        string tempZipFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");
        try
        {
            await using (FileStream zipStream = new FileStream(tempZipFile, FileMode.Create))
            {
                using (
                    ZipArchive zipArchive = new ZipArchive(
                        zipStream,
                        ZipArchiveMode.Create,
                        leaveOpen: false
                    )
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
            Buffer.BlockCopy(
                src: fileNameLengthBytes,
                srcOffset: 0,
                dst: payload,
                dstOffset: 0,
                count: fileNameLengthBytes.Length
            );
            Buffer.BlockCopy(
                src: fileNameBytes,
                srcOffset: 0,
                dst: payload,
                dstOffset: fileNameLengthBytes.Length,
                count: fileNameBytes.Length
            );
            Buffer.BlockCopy(
                src: zipData,
                srcOffset: 0,
                dst: payload,
                dstOffset: fileNameLengthBytes.Length + fileNameBytes.Length,
                count: zipData.Length
            );

            using TcpClient client = new TcpClient(ip, port);
            await using NetworkStream ns = client.GetStream();

            await ns.WriteAsync(payload, 0, payload.Length);
            return Path.GetFileName(tempZipFile);
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
