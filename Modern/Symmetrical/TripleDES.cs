namespace OldCrypt.Library.Modern.Symmetrical
{
	public class TripleDES : SymmetricalCipher
	{
		public TripleDES()
		{
			Algorithm = System.Security.Cryptography.TripleDES.Create();
		}

		public TripleDES(byte[] key, byte[] iv)
		{
			Algorithm = System.Security.Cryptography.TripleDES.Create();

			Key = key;
			IV = iv;
		}
	}
}
