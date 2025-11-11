namespace DeepSigma.ElectoralSystem.Enums;

/// <summary>
/// Voting rules used in decision-making processes.
/// Also known as electoral systems or voting systems.
/// </summary>
public enum ElectoralSystemType
{
    /// <summary>
    /// Simple plurality voting rule where the candidate with the most votes wins.
    /// Does not require a majority to win.
    /// Pros: Simple to understand and implement, and often leads to stable governments.
    /// Con: Can produce winners who do not have majority support and often leads to two-party systems.
    /// </summary>
    PluralityFirstPastthePost,
    /// <summary>
    /// Majority voting rule where if no candidate receives a majority of votes, the top two candidates proceed to a runoff election.
    /// Also known as two-round system and often leads to two-party systems.
    /// Pros: Ensures the winner has majority support and simplifies voter choice in the runoff.
    /// Con: Can lead to strategic voting in the first round and may require voters to return to the polls for a second round.
    /// </summary>
    PluralityTwoRoundRunOff,
    /// <summary>
    /// Preferential voting rule where voters rank candidates in order of preference.
    /// Perferential voting is also known as ranked-choice voting or instant-runoff voting.
    /// Candidates with the fewest first-choice votes are eliminated and their votes redistributed until one candidate has a majority (>50%).
    /// Pros: Encourages the consensus choice among voters rather than polarizing candidates.
    /// Cons: Produces more complex ballots and counting processes, which may confuse some voters.
    /// Also, may not always elect the Condorcet winner (the candidate who would win against each other candidate in a head-to-head matchup).
    /// </summary>
    PreferentialInstantRunoff,
    /// <summary>
    /// Preferential voting rule where voters rank candidates in order of preference and points are assigned based on the ranking.
    /// For example, in a 3-candidate race a first-place vote might be worth 2 points, a second-place vote 1 point, and a third-place vote 0 points.
    /// The candidate with the highest total points wins.
    /// Pros: Reflects the overall preference of the electorate more accurately than simple plurality systems.
    /// Cons: Can be more complex to understand and implement, and may still be susceptible to strategic voting.
    /// </summary>
    PerferentialBordaCount,
    /// <summary>
    /// Score-based voting rule where voters assign scores to candidates form a predefined range (e.g., 0-5).
    /// Candidates with the highest total scores win.
    /// Pros: Encourages more nuanced voting and reduces the impact of strategic voting.
    /// Cons: Encourages tactical voting or extreme scoring to maximize impact.
    /// </summary>
    ScoreBased,
    /// <summary>
    /// Proportional representation voting rule where seats are allocated based on the proportion of votes each party receives.
    /// Pros: More accurately reflects the diversity of voter preferences and encourages multi-party systems.
    /// Cons: Can lead to fragmented legislatures and coalition governments, which may be less stable.
    /// Also, can lead to coalition governments and less individual accountability that may not reflect the preferences of the majority of voters.
    /// </summary>
    ProportionalRepresentation,
    /// <summary>
    /// Proportional representation voting rule where voters rank candidates in order of preference and candidates are elected through a series of vote transfers.
    /// Quota-based system where candidates must reach a certain threshold of votes to be elected and excess votes are transferred to other candidates based on voter preferences.
    /// Pros: Combines the benefits of proportional representation with preferential voting, allowing for more nuanced voter preferences.
    /// Cons: Complex counting process that can be difficult to understand and implement.
    /// </summary>
    ProportionalRepresentationSingleTransferableVote,
    /// <summary>
    /// Pairwise comparison voting rule where candidates are compared head-to-head in a series of matchups.
    /// Also known as Condorcet method.
    /// Pros: Identifies the most broadly acceptable candidate and reduces the impact of vote splitting.
    /// Cons: Can lead to cycles or ties, making it difficult to determine a clear winner. 
    /// How? Humans do not always have transitive preferences which can lead to illogical outcomes like A > B, B > C, but C > A.
    /// </summary>
    PairwiseComparison
}
