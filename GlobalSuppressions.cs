// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "The point of this library is to demonstrate weak cryptographic algorithms")]
[assembly: SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "The point of this library is to demonstrate weak cryptographic algorithms")]
[assembly: SuppressMessage("Security", "CA5358:Review cipher mode usage with cryptography experts", Justification = "The point of this library is to demonstrate weak cryptographic algorithms")]
[assembly: SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "Multidimensional arrays are used to properly simulate tables")]
[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Arrays are returned intentionally")]
