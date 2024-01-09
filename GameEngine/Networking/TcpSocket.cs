using System.Net;
using System.Net.Sockets;

namespace GameEngine.Networking;

internal class TcpSocket
{
	private readonly TcpClient tcpClient = new TcpClient();
	private readonly EncryptionHandler encryptionHandler = new EncryptionHandler();

	public TcpSocket() { }

	public async Task<bool> Connect(string ipAddress, int port)
	{
		try
		{
			await tcpClient.ConnectAsync(IPAddress.Parse(ipAddress), port);
			return true;
		}
		catch { }
		return false;
	}
}