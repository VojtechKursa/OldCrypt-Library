namespace OldCrypt_Library.Hashing
{
	public class SHA1 : Hash
	{
		public SHA1()
		{
			hashAlgorithm = System.Security.Cryptography.SHA1.Create();
		}
	}
}
