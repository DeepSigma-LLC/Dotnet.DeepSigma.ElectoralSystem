
using DeepSigma.General;
using System.Collections.Immutable;

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
    ImmutableHashSet<Vote<VoteDetails>> SubmitedVotes { get; init; } = [];

    /// <summary>
    /// Adds a single vote to the collection.
    /// </summary>
    /// <param name="vote"></param>
    internal void Add(Vote<VoteDetails> vote)
    {
        if (vote.IsVoteValid() == false) return;

        SubmitedVotes.Add(vote); 
    }

    /// <summary>
    /// Adds a set of votes to the collection.
    /// </summary>
    /// <param name="votes"></param>
    internal void Add(HashSet<Vote<VoteDetails>> votes)
    {
        foreach (var vote in votes)
        {
            Add(vote);
        }
    }

    /// <summary>
    /// Tallies the votes in the collection and returns a dictionary with the results.
    /// </summary>
    /// <returns></returns>
    internal Dictionary<VoteDetails, int> TallyVotes()
    {
        Dictionary<VoteDetails, int> voteCount = [];
        foreach (var vote in SubmitedVotes)
        {
            if (voteCount.ContainsKey(vote.Details))
            {
                voteCount[vote.Details]++;
            }
            else
            {
                voteCount[vote.Details] = 1;
            }
        }
        return voteCount;
    })
}
