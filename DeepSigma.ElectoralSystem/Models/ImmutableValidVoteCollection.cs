
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
    HashSet<Vote<VoteDetails>> SubmitedVotes { get; init; } = [];

    /// <summary>
    /// Adds a single vote to the collection.
    /// </summary>
    /// <param name="vote"></param>
    internal Exception? Add(Vote<VoteDetails> vote)
    {
        if (!vote.IsVoteValid()) return new Exception("Vote is invalid.");
        if(VoterHasVoted(vote.VoterInfo)) return new Exception("Voter has already voted.");

        SubmitedVotes.Add(vote); 
        return default;
    }

    /// <summary>
    /// Checks if a voter has already voted.
    /// </summary>
    /// <param name="voter"></param>
    /// <returns></returns>
    internal bool VoterHasVoted(VoterInfo voter)
    {
        var votes = SubmitedVotes.Where(v => v.VoterInfo.VoterPublicKey.SequenceEqual(voter.VoterPublicKey));
        return votes.Any();
    }

    /// <summary>
    /// Tallies the votes in the collection and returns a dictionary with the results.
    /// </summary>
    /// <returns></returns>
    internal Dictionary<VoteDetails, int> TallyVotes()
    {
        Dictionary<VoteDetails, int> vote_tally = [];
        foreach (Vote<VoteDetails> vote in SubmitedVotes)
        {
            if (vote_tally.ContainsKey(vote.Details))
            {
                vote_tally[vote.Details]++;
            }
            else
            {
                vote_tally[vote.Details] = 1;
            }
        }
        return vote_tally;
    }
}
