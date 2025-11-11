using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Represents a collection of votes cast by voters.
/// </summary>
/// <typeparam name="VoteDetails"></typeparam>
public class VoteCollection<VoteDetails>() where VoteDetails : IDeterministicObjectOutput
{
    /// <summary>
    /// The voting system used for tallying votes.
    /// </summary>
    public Enum.ElectoralSystem VotingSystem { get; } = Enum.ElectoralSystem.PluralityFirstPastthePost;

    /// <summary>
    /// The set of individually submitted votes.
    /// </summary>
    ImmutableValidVoteCollection<VoteDetails> Votes { get; } = new();

    /// <summary>
    /// Adds a new vote to the collection.
    /// </summary>
    /// <param name="vote"></param>
    public void Add(Vote<VoteDetails> vote)
    {
        Votes.Add(vote);
    }

    /// <summary>
    /// Adds a range of votes to the collection.
    /// </summary>
    /// <param name="votes"></param>
    public void AddRange(HashSet<Vote<VoteDetails>> votes)
    {
        Votes.Add(votes);
    }

    /// <summary>
    /// Gets the complete vote count for all votes.
    /// </summary>
    /// <returns></returns>
    public Dictionary<VoteDetails, int> GetAllVoteCount()
    {
        return Votes.TallyVotes();
    }

    /// <summary>
    /// The total number of votes in the collection.
    /// </summary>
    public int TotalVotes => GetAllVoteCount().Values.Sum();

    /// <summary>
    /// Gets the vote count for a specific vote.
    /// </summary>
    /// <param name="Vote"></param>
    /// <returns></returns>
    public int GetVoteCountByVote(VoteDetails Vote)
    {
        bool found = GetAllVoteCount().TryGetValue(Vote, out int count);
        return found ? count : 0;
    }

    /// <summary>
    /// Gets the winning vote, or null in case of a tie.
    /// </summary>
    /// <returns></returns>
    public VoteDetails? GetTopVote()
    {
        Dictionary<VoteDetails, int> VoteCount = GetAllVoteCount();

        switch (VoteCount.Count)
        {
            case 0:
                return default;
            case 1:
                return VoteCount.Keys.First();
            default:
                List<(VoteDetails vote, int vote_count)> top_votes = VoteCount
                    .OrderByDescending(kvp => kvp.Value).Take(2)
                    .Select(kvp => (kvp.Key, kvp.Value)).ToList();

                if (top_votes[0].vote_count > top_votes[1].vote_count)
                {
                    return top_votes[0].vote;
                }
                return default;
        }
    }

    /// <summary>
    /// Gets the majority vote, or null if no majority exists.
    /// </summary>
    /// <returns></returns>
    public VoteDetails? GetMajorityVote()
    {
        Dictionary<VoteDetails, int> VoteCount = GetAllVoteCount();

        switch(VoteCount.Count)
        {
            case 0:
                return default;
            case 1:
                return VoteCount.Keys.First();
            default:
                (VoteDetails top_vote, int vote_count) = VoteCount.OrderByDescending(kvp => kvp.Value).First();
                if (vote_count > TotalVotes / 2)
                {
                    return top_vote;
                }
                return default; // No majority
        }
    }
}
