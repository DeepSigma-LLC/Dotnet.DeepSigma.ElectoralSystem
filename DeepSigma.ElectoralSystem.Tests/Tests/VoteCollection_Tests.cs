using Xunit;
using DeepSigma.ElectoralSystem.Models;
using DeepSigma.ElectoralSystem.Tests.Models;
using System.Security.Cryptography;
using DeepSigma.General.Utilities;
using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Tests.Tests;

public class VoteCollection_Tests
{
    private static ECDsa ecc = ECDsa.Create(ECCurve.NamedCurves.nistP256);

    private static VoterInfo GetVoterInfo()
    {
        return new VoterInfo("Alice", ecc.ExportSubjectPublicKeyInfo(), Enum.PublicKeyCryptographyAlgorithm.EllipticCurve);
    }

    [Fact]
    public void SingleVote_Tests()
    {
        VoteCollection<VoteExample> votes = new();
        VoteExample vote_details = new(1, 2, 22);

        byte[] signed_vote_details = CryptoUtilities.EllipticCurveDigitalSignData(vote_details.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);
        Vote<VoteExample> vote1 = new(GetVoterInfo(), vote_details, signed_vote_details, HashAlgorithmName.SHA256);
        
        votes.Add(vote1);

        Assert.NotNull(votes);
        Assert.Equal(1, votes.TotalVotes);
        Assert.Equal(1, votes.GetVoteCountByVote(vote_details));
        Assert.Equal(vote_details, votes.GetTopVote());
        Assert.Equal(vote_details, votes.GetMajorityVote());
    }

    [Fact]
    public void MultipleVote_Tests_NoTopVote()
    {
        VoteCollection<VoteExample> votes = new();
        VoteExample vote_details = new(1, 2, 22);
        VoteExample vote_details2 = new(2, 3, 33);
        VoteExample vote_details3 = new(3, 4, 44);

        byte[] signed_vote_details = CryptoUtilities.EllipticCurveDigitalSignData(vote_details.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);
        byte[] signed_vote_details2 = CryptoUtilities.EllipticCurveDigitalSignData(vote_details2.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);
        byte[] signed_vote_details3 = CryptoUtilities.EllipticCurveDigitalSignData(vote_details3.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);

        Vote<VoteExample> vote1 = new(GetVoterInfo(), vote_details, signed_vote_details, HashAlgorithmName.SHA256);
        Vote<VoteExample> vote2 = new(GetVoterInfo(), vote_details2, signed_vote_details2, HashAlgorithmName.SHA256);
        Vote<VoteExample> vote3 = new(GetVoterInfo(), vote_details3, signed_vote_details3, HashAlgorithmName.SHA256);

        votes.Add(vote1);
        votes.Add(vote2);
        votes.Add(vote3);

        Assert.NotNull(votes);
        Assert.Equal(3, votes.TotalVotes);
        Assert.Equal(1, votes.GetVoteCountByVote(vote_details));
        Assert.Equal(1, votes.GetVoteCountByVote(vote_details2));
        Assert.Equal(1, votes.GetVoteCountByVote(vote_details3));

        // No top vote since all have same count
        Assert.Null(votes.GetTopVote());
        // No majority vote since no vote has more than half the votes
        Assert.Null(votes.GetMajorityVote());
    }


    [Fact]
    public void MultipleVote_Tests_WithTopVote_And_Majority()
    {
        VoteCollection<VoteExample> votes = new();

        VoteExample vote_details = new(1, 2, 22);
        VoteExample vote_details2 = new(2, 3, 33);

        byte[] signed_vote_details = CryptoUtilities.EllipticCurveDigitalSignData(vote_details.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);
        byte[] signed_vote_details2 = CryptoUtilities.EllipticCurveDigitalSignData(vote_details2.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);
        byte[] signed_vote_details3 = CryptoUtilities.EllipticCurveDigitalSignData(vote_details2.ToDeterministicHash(HashAlgorithmName.SHA256), ecc);

        Vote<VoteExample> vote1 = new(GetVoterInfo(), vote_details, signed_vote_details, HashAlgorithmName.SHA256);
        Vote<VoteExample> vote2 = new(GetVoterInfo(), vote_details2, signed_vote_details2, HashAlgorithmName.SHA256);
        Vote<VoteExample> vote3 = new(GetVoterInfo(), vote_details, signed_vote_details3, HashAlgorithmName.SHA256);
        votes.Add(vote1);
        votes.Add(vote2);
        votes.Add(vote3);

        Assert.NotNull(votes);
        Assert.Equal(3, votes.TotalVotes);
        Assert.Equal(2, votes.GetVoteCountByVote(vote_details));
        Assert.Equal(1, votes.GetVoteCountByVote(vote_details2));

        // Top vote should be vote_details since it has the highest count
        Assert.Equal(vote_details, votes.GetTopVote());
        // Majority vote should be vote_details since it has more than half the votes
        Assert.Equal(vote_details, votes.GetMajorityVote());
    }

}
