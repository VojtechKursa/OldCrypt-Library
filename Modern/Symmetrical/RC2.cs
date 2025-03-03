namespace OldCrypt_Library.Modern.Symmetrical
{
	public class RC2 : SymmetricalCipher
	{
		public RC2()
		{
			algorithm = System.Security.Cryptography.RC2.Create();
		}

		public RC2(byte[] key, byte[] iv)
		{
			algorithm = System.Security.Cryptography.RC2.Create();

			Key = key;
			IV = iv;
		}
	}
}
