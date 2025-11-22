
namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Represents a collection of votes cast by voters.
/// </summary>
/// <typeparam name="VoteDetails"></typeparam>
public class VoteCollection<VoteDetails>() where VoteDetails : class
{
    /// <summary>
    /// The set of individually submitted votes.
    /// </summary>
    ImmutableValidVoteCollection<VoteDetails> Votes { get; } = new();

    /// <summary>
    /// Adds a new vote to the collection.
    /// </summary>
    /// <param name="vote"></param>
    public Exception? Add(Vote<VoteDetails> vote)
    {
        return Votes.Add(vote);
    }

    /// <summary>
    /// Adds a range of votes to the collection.
    /// </summary>
    /// <param name="votes"></param>
    public bool TryAddRange(HashSet<Vote<VoteDetails>> votes)
    {
        bool errored = false;
        foreach(var vote in votes)
        {
            Exception? error = Votes.Add(vote);
            if (error is not null) errored = true;
        }
        return errored;
    }

    /// <summary>
    /// Gets the complete vote count for all votes.
    /// </summary>
    /// <returns></returns>
    public Dictionary<VoteDetails, (int VoteCount, decimal WeightedVoteTotal)> TallyVotes()
    {
        return Votes.TallyVotes();
    }

    /// <summary>
    /// The total number of votes in the collection.
    /// </summary>
    public int TotalVotes => Votes.GetValidVotes().Count;

    /// <summary>
    /// The total weighted votes in the collection.
    /// </summary>
    public decimal TotalWeightedVotes => TallyVotes().Values.Select(vc => vc.WeightedVoteTotal).Sum();

    /// <summary>
    /// Gets the vote count for a specific vote.
    /// </summary>
    /// <param name="Vote"></param>
    /// <returns></returns>
    public (int VoteCount, decimal WeightedTotal) GetVoteCountByVote(VoteDetails Vote)
    {
        bool found = TallyVotes().TryGetValue(Vote, out (int, decimal) result);
        return found ? result : (0, 0);
    }

    /// <summary>
    /// Gets the top vote, or null in case of a tie.
    /// </summary>
    /// <returns></returns>
    public VoteDetails? GetTopVote()
    {
        Dictionary<VoteDetails, (int VoteTotal, decimal WeightedVoteTotal)> VoteCount = TallyVotes();

        switch (VoteCount.Count)
        {
            case 0:
                return default;
            case 1:
                return VoteCount.Keys.First();
            default:
                List<(VoteDetails vote, decimal weighted_total)> top_votes = VoteCount
                    .OrderByDescending(kvp => kvp.Value.WeightedVoteTotal).Take(2)
                    .Select(kvp => (kvp.Key, kvp.Value.WeightedVoteTotal)).ToList();

                if (top_votes[0].weighted_total > top_votes[1].weighted_total)
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
        var result = GetMajorityVoteUsingCustomThreshold(0.5);
        return result.result; // Ignore error for this method
    }

    /// <summary>
    /// Gets the majority vote using a custom threshold, or null if no majority exists.
    /// </summary>
    /// <param name="majority_threshold_percentage"> The custom threshold for majority. Defaults to 0.5 (aka 50%).</param>
    /// <returns></returns>
    public (VoteDetails? result, Exception? error) GetMajorityVoteUsingCustomThreshold(double majority_threshold_percentage =  0.5)
    {
        if (majority_threshold_percentage < 0 || majority_threshold_percentage > 1)
        {
            return (default, new ArgumentOutOfRangeException(nameof(majority_threshold_percentage), "Majority threshold percentage must be between 0 (inclusive) and 1 (inclusive)."));
        }

        Dictionary<VoteDetails, (int VoteCount, decimal WeightedVoteTotal)> VoteCount = TallyVotes();
        decimal majority_threshold_amount = TotalWeightedVotes * (decimal)majority_threshold_percentage;
        switch (VoteCount.Count)
        {
            case 0:
                return (default, null);
            case 1:
                return (VoteCount.Keys.First(), null);
            default:
                (VoteDetails top_vote, (int vote_count, decimal weighted_vote_total)) = VoteCount.OrderByDescending(kvp => kvp.Value.WeightedVoteTotal).First();
                if (weighted_vote_total > majority_threshold_amount)
                {
                    return (top_vote, null);
                }
                return (default, null); // No majority
        }
    }

    /// <summary>
    /// Gets the unanimous vote, or null if not all votes are the same.
    /// </summary>
    /// <returns></returns>
    public VoteDetails? GetUnanimousVote()
    {
        Dictionary<VoteDetails, (int VoteCount, decimal WeightedVoteTotal)> VoteCount = TallyVotes();
        if (VoteCount.Count == 1)
        {
            return VoteCount.Keys.First();
        }
        return default;
    }
}
