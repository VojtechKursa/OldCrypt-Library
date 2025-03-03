namespace OldCrypt.Library.Hashing
{
	public class SHA384 : Hash
	{
		public SHA384()
		{
			HashAlgorithm = System.Security.Cryptography.SHA384.Create();
		}
	}
}
