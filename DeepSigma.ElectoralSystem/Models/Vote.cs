using DeepSigma.General.Utilities;
using System.Security.Cryptography;
using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Models;


/// <summary>
/// Represents a casted vote.
/// </summary>
/// <typeparam name="VoteDetails"></typeparam>
public record class Vote<VoteDetails>(VoterInfo VoterInfo, VoteDetails Details, byte[] SignedVoteHash) where VoteDetails : IDeterministicObjectOutput
{
    private HashAlgorithmName HashAlgorithmName { get; set; } = HashAlgorithmName.SHA256;

    /// <summary>
    /// Validates the vote by verifying the digital signature using the voter's public key.
    /// </summary>
    /// <returns></returns>
    public bool IsVoteValid()
    {
        ECDsa ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(VoterInfo.VoterPublicKey, out int read_bytes);

        if (read_bytes != VoterInfo.VoterPublicKey.Length) return false;

        byte[] hashed_data = Details.ToDeterministicHash(HashAlgorithmName);

        return CryptoUtilities.EllipticCurveDigitalVerifyData(hashed_data, SignedVoteHash, ecdsa, HashAlgorithmName);
    }

}
