using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HelpDesk.Bll.Components
{
    public sealed class PasswordGenerator
    {
        private const int SaltSize = 16, HashSize = 20, HashIter = 1000;
        private readonly byte[] _salt, _hash;

        public PasswordGenerator(string password)
        {
            _salt = new byte[SaltSize];
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(_salt);
                using (var rfc = new Rfc2898DeriveBytes(password, _salt, HashIter))
                {
                    _hash = rfc.GetBytes(HashSize);
                }
            }
        }

        public PasswordGenerator(byte[] hashBytes)
        {
            _salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, _salt, 0, SaltSize);
            _hash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, _hash, 0, HashSize);
        }

        public PasswordGenerator(byte[] hash, byte[] salt) : this(hash)
        {
            // hash already contain salt
        }

        public byte[] GetSalt()
        {
            return (byte[])_salt.Clone();
        }

        public byte[] GetHash()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }

        public bool Verify(string password)
        {
            using (var rfc = new Rfc2898DeriveBytes(password, _salt, HashIter))
            {
                byte[] test = rfc.GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                {
                    if (test[i] != _hash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
