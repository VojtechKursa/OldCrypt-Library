namespace OldCrypt_Library.Hashing
{
	public class SHA512 : Hash
	{
		public SHA512()
		{
			hashAlgorithm = System.Security.Cryptography.SHA512.Create();
		}
	}
}
