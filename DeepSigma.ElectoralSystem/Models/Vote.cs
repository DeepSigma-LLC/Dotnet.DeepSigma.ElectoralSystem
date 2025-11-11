using DeepSigma.General.Utilities;
using System.Security.Cryptography;
using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Represents a casted vote.
/// </summary>
/// <typeparam name="VoteDetails"></typeparam>
/// <param name="VoterInfo">Information about the voter.</param>
/// <param name="Details">The details of the vote.</param>
/// <param name="SignedVoteHash">The digitally signed hash of the vote details.</param>
/// <param name="HashAlgorithm">The hash algorithm used for signing the vote.</param>
public record class Vote<VoteDetails>(VoterInfo VoterInfo, VoteDetails Details, byte[] SignedVoteHash, HashAlgorithmName HashAlgorithm) where VoteDetails : IDeterministicObjectOutput
{
    /// <summary>
    /// Validates the vote by verifying the digital signature using the voter's public key.
    /// </summary>
    /// <returns></returns>
    public bool IsVoteValid()
    {
        ECDsa ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(VoterInfo.VoterPublicKey, out int read_bytes);

        if (read_bytes != VoterInfo.VoterPublicKey.Length) return false;

        byte[] hashed_data = Details.ToDeterministicHash(HashAlgorithm);

        return CryptoUtilities.EllipticCurveDigitalVerifyData(hashed_data, SignedVoteHash, ecdsa, HashAlgorithm);
    }

}
