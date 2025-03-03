namespace OldCrypt.Library.Hashing
{
	public class MD5 : Hash
	{
		public MD5()
		{
			HashAlgorithm = System.Security.Cryptography.MD5.Create();
		}
	}
}
