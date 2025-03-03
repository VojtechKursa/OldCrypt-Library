using System.Security.Cryptography;

namespace OldCrypt.Library.Modern.Symmetrical
{
	public class AES : SymmetricalCipher
	{
		public AES()
		{
			Algorithm = Aes.Create();
		}

		public AES(byte[] key, byte[] iv)
		{
			Algorithm = Aes.Create();

			Key = key;
			IV = iv;
		}
	}
}
