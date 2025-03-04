# OldCrypt Library

## Introduction

This is a library that contains algorithms for encrypting and decrypting data (text/binary) using mostly old encryption methods.
It's by no means a secure encryption library and was never meant to be. It's primary purpose is education/entertainment by allowing the user to explore various pre-digital cryptographic methods.

The library was originally written for .NET Framework 4.7.2, but has since been rewritten to target .NET Standard 2.0.

## Usage of this library

### Structure

All classes in this library can be found under the `OldCrypt.Library` namespace.
The namespace includes other namespaces that have the following contents:

- Old - Old (pre-digital) ciphers
  - Substitution - Ciphers based on substitution
  - Transposition - Ciphers based on transposition
- Modern - Mostly modern (digital ciphers). Note: Despite this namespace containing "Modern" ciphers, the fact that they're digital doesn't necessarily mean they're secure and shouldn't be used as such.
  - Asymmetrical - Digital asymmetrical ciphers
  - Symmetrical - Digital symmetrical ciphers
- Hashing - Utilities for digital hashing
- Converters - Utilities for conversions between formats
- Data - Data structures used by ciphers
- Exceptions - Exceptions thrown by this library

### Ciphers

Every cipher is represented by a class inheriting from the `Cipher` class, that can be found in this library's root namespace.
The classes implement methods for encryption and decryption of textual and binary data:

- `string Encrypt(string text)`
- `string Decrypt(string text)`
- `byte[] Encrypt(byte[] data)`
- `byte[] Decrypt(byte[] data)`

Each class will require different parameters passed to the constructor based on what parameters the cipher the given class represents operates with.

### Usage

Initialize the class which represents the desired cipher. For example the following code will initialize a new instance of the class representing the Caesar cipher with the shift (a) parameter set to 5.

```cs
var caesar = new Caesar(5);
```

Then we can use the object to encrypt or decrypt data.

```cs
string encrypted = caesar.Encrypt("hello");     // encrypted: "mjqqt"
string decrypted = caesar.Decrypt(encrypted);   // decrypted: "hello"
```

### Note on binary Encrypt and Decrypt methods

The `byte[] Encrypt(byte[] data)` and `byte[] Decrypt(byte[] data)` can be used to encrypt or decrypt binary data based on the same principle used to encrypt or decrypt textual data.
Despite the original pre-digital ciphers being only intended for text, I've tried to extend their principle to allow them to work with binary data, as long as the binary version didn't change the original principle of the cipher.

However, the principle of **some** ciphers is incompatible with digital data, since their logic is based on working with text.
In such cases, when the binary `Encrypt` or `Decrypt` methods are called on ciphers that, by principle, don't allow working with binary data, the `CipherUnavailableException` will be thrown by the method.

Example:

```cs
new Caesar(5).Encrypt(new byte[] {1, 1, 1}); // encrypted: (byte[]) [6, 6, 6]

new Playfair().Encrypt(new byte[] {1, 1, 1}); // CipherUnavailableException thrown
```

## License

This library is licensed under the **GNU General Public License v3**.

## Few author's words

I've created this library when creating my program [OldCrypt](https://github.com/VojtechKursa/OldCrypt-GUI) and decided to make the cryptographical library separate, so anyone can easily use it within their programs.
I'm not really sure how anyone's gonna use it, but I guess someone somewhere could fancy a good old *Playfair* in their program.

## Building the library

### Visual Studio

Opening either the *.csproj* or *.sln* file in the root of this repository in Visual Studio and build the project.
Alternatively you can download a build from the [releases section of the GitHub repository](https://github.com/VojtechKursa/OldCrypt-Library/releases).

### .NET CLI

Run the following command in the root of the repository:

```sh
dotnet build
```

## Contributing

This library is licensed under the **GNU General Public License v3** so anyone can use it for free and modify it as long as that person follows the conditions stated by the license.
If you have suggestions that would optimize or otherwise improve this library feel free to share them by creating an issue or submitting a pull request with your implemented ideas in the GitHub repository (see section *Source code & Repository*).
If you want to add something into/modify this library's code, please refer to the [CONTRIBUTING.md](/CONTRIBUTING.md) file for further information.

## Source code & Repository

The source code for this library and it's public repository can be found on [GitHub](https://github.com/VojtechKursa/OldCrypt-Library).
