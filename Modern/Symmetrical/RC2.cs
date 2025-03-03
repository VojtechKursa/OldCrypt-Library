namespace OldCrypt.Library.Modern.Symmetrical
{
	public class RC2 : SymmetricalCipher
	{
		public RC2()
		{
			Algorithm = System.Security.Cryptography.RC2.Create();
		}

		public RC2(byte[] key, byte[] iv)
		{
			Algorithm = System.Security.Cryptography.RC2.Create();

			Key = key;
			IV = iv;
		}
	}
}
