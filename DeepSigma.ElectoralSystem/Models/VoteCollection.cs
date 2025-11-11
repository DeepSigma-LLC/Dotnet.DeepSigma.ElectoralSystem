using DeepSigma.ElectoralSystem.Enum;
using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Represents a collection of votes cast by voters.
/// Note: This 
/// </summary>
/// <typeparam name="T"></typeparam>
public class VoteCollection<T>() where T : IDeterministicObjectOutput
{
    /// <summary>
    /// The voting system used for tallying votes.
    /// </summary>
    public VotingSystem VotingSystem { get; } = VotingSystem.PluralityFirstPastthePost;

    /// <summary>
    /// The set of individually submitted votes.
    /// </summary>
    HashSet<Vote<T>> SubmitedVotes { get; init; } = [];

    /// <summary>
    /// The mapping of votes to their respective counts.
    /// </summary>
    Dictionary<T, int> VoteCount { get; init; } = [];

    /// <summary>
    /// Adds a new vote to the collection.
    /// </summary>
    /// <param name="nodeVote"></param>
    public void Add(Vote<T> nodeVote)
    {
        if (nodeVote.IsVoteValid() == false) return;

        SubmitedVotes.Add(nodeVote);

        if (VoteCount.ContainsKey(nodeVote.VoteDetails))
        {
            VoteCount[nodeVote.VoteDetails]++;
        }
        else
        {
            VoteCount[nodeVote.VoteDetails] = 1;
        }
    }

    /// <summary>
    /// Adds a range of votes to the collection.
    /// </summary>
    /// <param name="nodeVotes"></param>
    public void AddRange(IEnumerable<Vote<T>> nodeVotes)
    {
        foreach (var vote in nodeVotes)
        {
            Add(vote);
        }
    }

    /// <summary>
    /// The total number of votes in the collection.
    /// </summary>
    public int TotalVotes => VoteCount.Values.Sum();

    /// <summary>
    /// Gets the vote count for a specific vote.
    /// </summary>
    /// <param name="Vote"></param>
    /// <returns></returns>
    public int GetVoteCountByVote(T Vote)
    {
        return VoteCount.TryGetValue(Vote, out int count) ? count : 0;
    }

    /// <summary>
    /// Gets the winning vote, or null in case of a tie.
    /// </summary>
    /// <returns></returns>
    public T? GetWinningVote()
    {
        if (VoteCount.Count == 0) return default;
        int maxVotes = VoteCount.Values.Max();

        List<T> winningVotes = VoteCount.Where(kvp => kvp.Value == maxVotes).Select(kvp => kvp.Key).ToList();
        if (winningVotes.Count == 1)
        {
            return winningVotes[0];
        }
        return default; // Tie
    }
}
