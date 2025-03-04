# Contributing to the OldCrypt Library

The OldCrypt Library is open-source and anyone can modify it or add new code to it as long as they follow the conditions defined by the **GNU General Public License v3**.

## Standards
Even though anyone can add or modify the code in any way they want. I'd like people to follow the standards I've written this library in, for consistency reasons.

1. Basic structure
	1. Every encryption method should be it's own non-static class that has the Cipher class or any other class that inherits from it as it's parent.
	2. The Encrypt and Decrypt methods shall be overwritten by code that returns the encrypted/decrypted text/data based on the desired encryption method.
	3. The EncryptFile and DecryptFile methods don't have to be overwritten, but are defined as virtual and can be overwritten, in case the default implementation doesn't suit your needs.
2. Requirements for methods
	1. The Encrypt and Decrypt methods for string should update the progress variable with the progress of encryption/decryption process as you see fit. This progress should be a double between 0 and 1.
	2. The Encrypt and Decrypt methods for byte[] are usually used by the EncryptFile and DecryptFile methods so you don't have to implement the progress updating there since it's implemented in the EncryptFile and DecryptFile methods. Except in situations where you would want to call these methods directly (not through EncryptFile/DecryptFile methods).
	3. When overwriting the EncryptFile and DecryptFile methods, you should update the progress variable. Ideally you want to pass the file into the EncryptBytes/DecryptBytes method in parts, not as a whole.