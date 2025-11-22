using DeepSigma.General;
using DeepSigma.Mathematics.Randomization;

namespace DeepSigma.ElectoralSystem.Models;

/// <summary>
/// Represents a raffle collection.
/// </summary>
/// <typeparam name="VoteDetails"></typeparam>
public class RaffleCollection<VoteDetails> where VoteDetails : class
{
    WeightedRandom<VoteDetails> VoteCollection { get; set; } = new();

    /// <summary>
    /// Adds a VoteDetails with a specified weight to the raffle collection.
    /// </summary>
    public void Add(Vote<VoteDetails> vote)
    {
        VoteCollection.AddItem(vote.Details, (double)vote.Weight);
    }

    /// <summary>
    /// Draws a random VoteDetails based on their weights.
    /// Note: Each call to this method may return different results. 
    /// It samples from the weighted distribution each time it is called.
    /// </summary>
    /// <returns></returns>
    public VoteDetails DrawRandom()
    {
        return VoteCollection.Next();
    }

    /// <summary>
    /// Draws multiple random VoteDetails based on their weights.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public IEnumerable<VoteDetails> DrawRandomMultiple(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return VoteCollection.Next();
        }
    }
}
