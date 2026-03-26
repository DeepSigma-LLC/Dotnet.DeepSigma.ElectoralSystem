# DeepSigma.ElectoralSystem

A general-purpose C# library for building vote-driven workflows, tallying results, and validating digitally signed ballots in .NET.

This package targets **.NET 10** and is published as **`DeepSigma.ElectoralSystem`**. It provides core models for representing voters and votes, a vote collection for tallying and winner detection, and a weighted raffle collection for randomized selection scenarios. The repository also includes xUnit tests for core vote-aggregation behavior.

## Highlights

- **Typed vote models** for `Vote`, `VoterInfo`, and vote collections
- **Signed-vote validation** using deterministic hashing plus public-key signature verification
- **Duplicate-voter protection** within the internal validated vote collection
- **Weighted voting support** via a `Weight` value on each vote
- **Common result helpers**:
  - total votes
  - weighted totals
  - top vote
  - majority vote
  - unanimous vote
- **Weighted raffle selection** for lottery-style or randomized outcomes
- **Enumerations for electoral-system categories** and supported public-key algorithm types

## Package metadata

- **Package ID:** `DeepSigma.ElectoralSystem`
- **Current version in the repo:** `1.2.0`
- **Target framework:** `net10.0`
- **License:** MIT

## What the library currently implements

At the core of the library are these public types:

- `VoterInfo` – voter identifier, public key bytes, and declared public-key algorithm
- `Vote<TVoteDetails>` – a signed vote with voter info, vote details, hash algorithm, and optional vote weight
- `VoteCollection<TVoteDetails>` – stores valid votes and exposes tallying and winner-selection helpers
- `RaffleCollection<TVoteDetails>` – weighted random selection based on vote weights

The repository also defines:

- `ElectoralSystemType` – categories such as first-past-the-post, runoff, instant runoff, Borda count, score-based voting, proportional representation, STV, and pairwise comparison
- `PublicKeyCryptographyAlgorithm` – `RSA`, `EllipticCurve`, and `DiffieHellman`

## Installation

Add the package from NuGet:

```bash
dotnet add package DeepSigma.ElectoralSystem
```

Or reference it in your project file:

```xml
<PackageReference Include="DeepSigma.ElectoralSystem" Version="1.2.0" />
```

## Quick start

### 1. Define a vote payload

Your vote payload type should be deterministic so it can be hashed consistently for signing and verification.

```csharp
using DeepSigma.General;

public record class ProposalVote(string ProposalId, bool Approve) : IDeterministicObjectOutput;
```

### 2. Create voter information

```csharp
using DeepSigma.ElectoralSystem.Enums;
using DeepSigma.ElectoralSystem.Models;
using System.Security.Cryptography;

using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

VoterInfo voter = new(
    VoterId: "alice",
    VoterPublicKey: ecdsa.ExportSubjectPublicKeyInfo(),
    PublicKeyAlgorithm: PublicKeyCryptographyAlgorithm.EllipticCurve
);
```

### 3. Sign the vote payload and create a vote

```csharp
using DeepSigma.ElectoralSystem.Models;
using DeepSigma.General.Utilities;
using System.Security.Cryptography;

ProposalVote details = new("proposal-123", true);

byte[] signedHash = CryptoUtilities.EllipticCurveDigitalSignData(
    details.ToDeterministicHash(HashAlgorithmName.SHA256),
    ecdsa
);

Vote<ProposalVote> vote = new(
    VoterInfo: voter,
    Details: details,
    SignedVoteHash: signedHash,
    HashAlgorithm: HashAlgorithmName.SHA256,
    Weight: 1
);
```

### 4. Add the vote to a collection and query results

```csharp
using DeepSigma.ElectoralSystem.Models;

VoteCollection<ProposalVote> votes = new();

Exception? error = votes.Add(vote);
if (error is not null)
{
    Console.WriteLine(error.Message);
    return;
}

Console.WriteLine($"Total votes: {votes.TotalVotes}");
Console.WriteLine($"Total weighted votes: {votes.TotalWeightedVotes}");

ProposalVote? top = votes.GetTopVote();
ProposalVote? majority = votes.GetMajorityVote();
ProposalVote? unanimous = votes.GetUnanimousVote();
```

## Weighted voting

Each `Vote<TVoteDetails>` includes a `Weight` parameter that defaults to `1`.

That means you can model scenarios such as:

- shareholder voting
- delegated or proxy voting
- committee voting with unequal member weights
- token- or stake-weighted governance experiments

Example:

```csharp
Vote<ProposalVote> weightedVote = new(
    VoterInfo: voter,
    Details: new ProposalVote("proposal-123", true),
    SignedVoteHash: signedHash,
    HashAlgorithm: HashAlgorithmName.SHA256,
    Weight: 12.5m
);
```

## Weighted raffle / randomized selection

`RaffleCollection<TVoteDetails>` lets you draw outcomes from a weighted distribution.

```csharp
using DeepSigma.ElectoralSystem.Models;

RaffleCollection<ProposalVote> raffle = new();
raffle.Add(vote);

ProposalVote winner = raffle.DrawRandom();

foreach (ProposalVote item in raffle.DrawRandomMultiple(3))
{
    Console.WriteLine(item);
}
```

This is useful for:

- lottery-based selection
- weighted sampling
- randomized governance experiments
- prize or incentive systems

## Result helpers in `VoteCollection<TVoteDetails>`

The main collection exposes these useful operations:

- `Add(vote)` – adds a vote and returns an exception when invalid
- `TryAddRange(votes)` – attempts to add many votes
- `TallyVotes()` – returns per-option vote counts and weighted totals
- `GetVoteCountByVote(option)` – returns count and weighted total for a given option
- `GetTopVote()` – returns the unique highest-weighted option, or `null` for a tie
- `GetMajorityVote()` – returns the option with more than 50% of weighted votes
- `GetMajorityVoteUsingCustomThreshold(threshold)` – custom majority threshold
- `GetUnanimousVote()` – returns the option only when all valid votes match

## Validation model

A vote is considered valid when its signature verifies against the voter’s public key and the deterministic hash of the vote details.

The internal validated-vote collection also rejects:

- invalid signatures
- multiple votes from the same voter public key

## Important implementation note

The library declares multiple public-key algorithm options in `PublicKeyCryptographyAlgorithm`, but the current concrete verification path in `Vote<TVoteDetails>.IsVoteValid()` imports the voter public key with `ECDsa.ImportSubjectPublicKeyInfo(...)` and verifies with elliptic-curve digital signatures.

In practice, that means **ECDSA-backed voting is the implemented verification flow shown in the current codebase**.

## Electoral system taxonomy

`ElectoralSystemType` documents the broader kinds of electoral systems the package is intended to support, including:

- plurality / first-past-the-post
- two-round runoff
- instant runoff / ranked-choice
- Borda count
- score-based voting
- proportional representation
- single transferable vote
- pairwise comparison / Condorcet-style approaches

This enum is currently best understood as a taxonomy / roadmap signal rather than a full set of built-in tally engines for each method.

## Testing

The repository includes **xUnit** tests covering core vote collection scenarios, including:

- single-vote behavior
- multi-vote tallying
- top-vote resolution
- majority detection
- unanimous-result detection

Run the test suite with:

```bash
dotnet test
```

## Project structure

```text
DeepSigma.ElectoralSystem/
├── DeepSigma.ElectoralSystem/
│   ├── Enums/
│   │   ├── ElectoralSystemType.cs
│   │   └── PublicKeyCryptographyAlgorithm.cs
│   └── Models/
│       ├── ImmutableValidVoteCollection.cs
│       ├── RaffleCollection.cs
│       ├── Vote.cs
│       ├── VoteCollection.cs
│       └── VoterInfo.cs
└── DeepSigma.ElectoralSystem.Tests/
    ├── Models/
    │   └── VoteExample.cs
    └── Tests/
        └── VoteCollection_Tests.cs
```

## When this library is a good fit

This package is a good fit when you need:

- a lightweight domain model for signed votes
- vote tallying with weighted totals
- majority, unanimous, and top-choice helpers
- a starting point for cryptographically verifiable voting workflows
- a reusable building block inside a larger governance, DAO, ballot, polling, or election platform

## Current boundaries

Based on the current repository contents, this package does **not** yet appear to ship complete end-to-end tally implementations for every electoral system named in `ElectoralSystemType`. Instead, the implemented core today is centered on:

- signed vote representation
- vote validation
- tallying and winner helpers
- weighted randomized selection

## License

MIT
