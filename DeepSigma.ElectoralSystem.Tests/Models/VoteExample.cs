
using DeepSigma.General;

namespace DeepSigma.ElectoralSystem.Tests.Models;

public record class VoteExample(int Id, int Vote1, int Vote2) : IDeterministicObjectOutput;

