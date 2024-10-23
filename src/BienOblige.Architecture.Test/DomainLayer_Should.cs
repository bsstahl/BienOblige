using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Xunit;

// using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BienOblige.Architecture.Test;

public class DomainLayer_Should
{
    Ruleset _ruleSet = new();

    [Fact]
    public void DependOnlyOnAppropriateLayers()
    {
        _ruleSet.CheckAllRules();
    }
}