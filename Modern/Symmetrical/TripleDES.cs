namespace OldCrypt_Library.Modern.Symmetrical
{
	public class TripleDES : SymmetricalCipher
	{
		public TripleDES()
		{
			algorithm = System.Security.Cryptography.TripleDES.Create();
		}

		public TripleDES(byte[] key, byte[] iv)
		{
			algorithm = System.Security.Cryptography.TripleDES.Create();

			Key = key;
			IV = iv;
		}
	}
}
