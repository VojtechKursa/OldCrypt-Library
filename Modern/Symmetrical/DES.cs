namespace OldCrypt.Library.Modern.Symmetrical
{
	public class DES : SymmetricalCipher
	{
		public DES()
		{
			Algorithm = System.Security.Cryptography.DES.Create();
		}

		public DES(byte[] key, byte[] iv)
		{
			Algorithm = System.Security.Cryptography.DES.Create();

			Key = key;
			IV = iv;
		}
	}
}
