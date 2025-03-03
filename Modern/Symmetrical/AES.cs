using System.Security.Cryptography;

namespace OldCrypt_Library.Modern.Symmetrical
{
	public class AES : SymmetricalCipher
	{
		public AES()
		{
			algorithm = Aes.Create();
		}

		public AES(byte[] key, byte[] iv)
		{
			algorithm = Aes.Create();

			Key = key;
			IV = iv;
		}
	}
}
