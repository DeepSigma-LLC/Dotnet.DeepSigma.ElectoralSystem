
using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// A collection of valid votes that cannot be modified after creation.
/// </summary>
/// <typeparam name="VoteDetails"></typeparam>
internal class ImmutableValidVoteCollection<VoteDetails> where VoteDetails : IDeterministicObjectOutput
{
    /// <summary>
    /// The set of individually submitted votes.
    /// </summary>
    HashSet<Vote<VoteDetails>> ValidVotes { get; init; } = [];

    /// <summary>
    /// The set of individually submitted votes.
    /// </summary>
    HashSet<Vote<VoteDetails>> InvalidVotes { get; init; } = [];

    /// <summary>
    /// Adds a single vote to the collection.
    /// </summary>
    /// <param name="vote"></param>
    internal Exception? Add(Vote<VoteDetails> vote)
    {
        if (!vote.IsVoteValid())
        {
            InvalidVotes.Add(vote);
            return new Exception("Vote is invalid.");
        }

        if (VoterHasVoted(vote.VoterInfo))
        {
            InvalidVotes.Add(vote);
            return new Exception("Voter has already voted.");
        }

        ValidVotes.Add(vote); 
        return default;
    }

    /// <summary>
    /// Gets the collection of invalid votes.
    /// </summary>
    /// <returns></returns>
    internal HashSet<Vote<VoteDetails>> GetInvalidVotes()
    {
        return InvalidVotes;
    }

    /// <summary>
    /// Gets the collection of valid votes.
    /// </summary>
    /// <returns></returns>
    internal HashSet<Vote<VoteDetails>> GetValidVotes()
    {
        return ValidVotes;
    }


    /// <summary>
    /// Checks if a voter has already voted.
    /// </summary>
    /// <param name="voter"></param>
    /// <returns></returns>
    internal bool VoterHasVoted(VoterInfo voter)
    {
        var votes = ValidVotes.Where(v => v.VoterInfo.VoterPublicKey.SequenceEqual(voter.VoterPublicKey));
        return votes.Any();
    }

    /// <summary>
    /// Tallies the votes in the collection and returns a dictionary with the results.
    /// </summary>
    /// <returns></returns>
    internal Dictionary<VoteDetails, (int VoteTotal, decimal WeightedVoteTotal)> TallyVotes()
    {
        Dictionary<VoteDetails, (int VoteTotal, decimal WeightedVoteTotal)> vote_tally = [];
        foreach (Vote<VoteDetails> vote in ValidVotes)
        {
            if (vote_tally.ContainsKey(vote.Details))
            {
                int vote_count = vote_tally[vote.Details].VoteTotal;
                decimal weighted_count = vote_tally[vote.Details].WeightedVoteTotal;
                vote_tally[vote.Details] = (vote_count++, weighted_count + vote.Weight);
                continue;
            }

            vote_tally[vote.Details] = (1, vote.Weight);
        }
        return vote_tally;
    }
}
