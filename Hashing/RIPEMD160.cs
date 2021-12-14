namespace OldCrypt_Library.Hashing
{
    public class RIPEMD160 : Hash
    {
        public RIPEMD160()
        {
            hashAlgorithm = System.Security.Cryptography.RIPEMD160.Create();
        }
    }
}
