namespace OldCrypt_Library.Modern.Symmetrical
{
    public class DES : SymmetricalCipher
    {
        public DES()
        {
            algorithm = System.Security.Cryptography.DES.Create();
        }

        public DES(byte[] key, byte[] iv)
        {
            algorithm = System.Security.Cryptography.DES.Create();

            Key = key;
            IV = iv;
        }
    }
}
