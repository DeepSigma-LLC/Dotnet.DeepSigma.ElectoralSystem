using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Contains information about a voter.
/// </summary>
public class VoterInfo : IDeterministicObjectOutput
{
    /// <summary>
    /// Unique identifier for the voter.
    /// </summary>
    public required string VoterId { get; set; }

    /// <summary>
    /// Public key of the voter..
    /// Used to verify digitally signed votes cast by the voter to ensure authenticity and integrity.
    /// </summary>
    public byte[] VoterPublicKey { get; set; } = [];

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"VoterId:{VoterId}, VoterPublicKey:{Convert.ToBase64String(VoterPublicKey)}";
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(VoterId, VoterPublicKey);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not VoterInfo other)
            return false;

        return VoterId == other.VoterId &&
               ((VoterPublicKey == null && other.VoterPublicKey == null) ||
                (VoterPublicKey != null && other.VoterPublicKey != null &&
                 VoterPublicKey.SequenceEqual(other.VoterPublicKey)));
    }
}
