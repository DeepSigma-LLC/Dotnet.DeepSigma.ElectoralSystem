using DeepSigma.ElectoralSystem.Enums;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Contains information about a voter.
/// </summary>
/// <param name="VoterId">Unique identifier for the voter.</param>
/// <param name="VoterPublicKey">Public key of the voter used to verify digitally signed votes.</param>
/// <param name="PublicKeyAlgorithm">The public key cryptography algorithm used.</param>
public record class VoterInfo(string VoterId, byte[] VoterPublicKey, PublicKeyCryptographyAlgorithm PublicKeyAlgorithm);
