namespace OldCrypt_Library.Hashing
{
    public class SHA384 : Hash
    {
        public SHA384()
        {
            hashAlgorithm = System.Security.Cryptography.SHA384.Create();
        }
    }
}
