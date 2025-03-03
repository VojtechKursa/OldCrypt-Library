namespace OldCrypt.Library.Hashing
{
	public class SHA512 : Hash
	{
		public SHA512()
		{
			HashAlgorithm = System.Security.Cryptography.SHA512.Create();
		}
	}
}
