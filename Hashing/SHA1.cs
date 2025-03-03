namespace OldCrypt.Library.Hashing
{
	public class SHA1 : Hash
	{
		public SHA1()
		{
			HashAlgorithm = System.Security.Cryptography.SHA1.Create();
		}
	}
}
