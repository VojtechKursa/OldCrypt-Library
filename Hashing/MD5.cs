namespace OldCrypt_Library.Hashing
{
    public class MD5 : Hash
    {
        public MD5()
        {
            hashAlgorithm = System.Security.Cryptography.MD5.Create();
        }
    }
}
