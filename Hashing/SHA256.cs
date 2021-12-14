namespace OldCrypt_Library.Hashing
{
    public class SHA256 : Hash
    {
        public SHA256()
        {
            hashAlgorithm = System.Security.Cryptography.SHA256.Create();
        }
    }
}
