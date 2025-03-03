namespace OldCrypt.Library.Hashing
{
	public class SHA256 : Hash
	{
		public SHA256()
		{
			HashAlgorithm = System.Security.Cryptography.SHA256.Create();
		}
	}
}
