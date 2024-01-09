using System.Security.Cryptography;

namespace GameEngine.Networking;

public class EncryptionHandler
{
	private readonly RSA rsa = RSA.Create();
	private readonly Aes aes = Aes.Create();

	public EncryptionHandler()
	{
		aes.Padding = PaddingMode.PKCS7;
		aes.Mode = CipherMode.CBC;
	}

	public byte[] EncryptAes(byte[] buffer)
	{
		try
		{
			return aes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
		}
		catch { throw new CryptographicException(); }
	}

	public byte[] EncryptRsa(byte[] buffer)
	{
		try
		{
			return rsa.Encrypt(buffer, RSAEncryptionPadding.OaepSHA256);
		}
		catch { throw new CryptographicException(); }
	}

	public byte[] DecryptAes(byte[] buffer)
	{
		try
		{
			return aes.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
		}
		catch { throw new CryptographicException(); }
	}

	public byte[] DecryptRsa(byte[] buffer)
	{
		try
		{
			return rsa.Decrypt(buffer, RSAEncryptionPadding.OaepSHA256);
		}
		catch { throw new CryptographicException(); }
	}

	public void ImportRsa(byte[] rsaPublicKey)
	{
		try
		{
			rsa.ImportRSAPublicKey(rsaPublicKey, out _);
		}
		catch { throw new CryptographicException(); }
	}

	public byte[] ExportRsa()
	{
		try
		{
			return rsa.ExportRSAPublicKey();
		}
		catch { throw new CryptographicException(); }
	}

	public void ImportAesPrivateKey(byte[] aesPrivateKey)
	{
		aes.Key = aesPrivateKey;
	}

	public void ImportAesIv(byte[] aesIv)
	{
		aes.IV = aesIv;
	}

	public byte[] ExportAesPrivateKey()
	{
		return aes.Key;
	}

	public byte[] ExportAesIv()
	{
		return aes.IV;
	}
}