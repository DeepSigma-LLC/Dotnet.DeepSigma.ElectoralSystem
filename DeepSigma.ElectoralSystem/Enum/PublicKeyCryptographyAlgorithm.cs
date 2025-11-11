
namespace DeepSigma.ElectoralSystem.Enum;

/// <summary>
/// Public Key Cryptography Algorithms
/// </summary>
public enum PublicKeyCryptographyAlgorithm
{
    /// <summary>
    /// RSA Algorithm stands for Rivest-Shamir-Adleman, which is one of the first public-key cryptosystems and is widely used for secure data transmission.
    /// It is based on the mathematical difficulty of factoring the product of two large prime numbers.
    /// It is commonly used for secure data transmission, digital signatures, and key exchange.
    /// Cons: Slower than symmetric key algorithms, requires larger key sizes for equivalent security.
    /// </summary>
    RSA,
    /// <summary>
    /// Elliptic Curve Cryptography (ECC) is based on the algebraic structure of elliptic curves over finite fields.
    /// It offers similar levels of security to RSA but with much smaller key sizes, making it more efficient.
    /// Cons: More complex mathematics, less widely adopted than RSA in some applications.
    /// </summary>
    EllipticCurve,
    /// <summary>
    /// Diffie-Hellman Key Exchange is a method for securely exchanging cryptographic keys over a public channel.
    /// It allows two parties to generate a shared secret key, which can then be used for symmetric encryption.
    /// Cons: The basic Diffie-Hellman algorithm does not provide authentication, making it vulnerable to man-in-the-middle attacks.
    /// </summary>
    DiffieHellman,
}
