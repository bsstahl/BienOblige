using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using BienOblige.Architecture.Test.Extensions;
using BienOblige.Architecture.Test.Enumerations;
using System.Collections;

using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.xUnit;
using System.Reflection;

namespace BienOblige.Architecture.Test;

internal class Ruleset : IEnumerable<IArchRule>
{
    private readonly List<IArchRule> _rules;

    // This needs to be updated whenever a new assembly is added until we find
    // a way to automatically load all assemblies in the solution.
    private static List<KeyValuePair<string, Layer>> Assemblies => new()
        {
            new ("BienOblige", Layer.Domain),
            new ("BienOblige.Execution", Layer.Domain),
            new ("BienOblige.Execution.Application", Layer.Application),
            new ("BienOblige.Execution.Data.Kafka", Layer.Infrastructure),
            new ("BienOblige.ApiService", Layer.Interface),
            new ("BienOblige.AppHost", Layer.Hosting),
            new ("BienOblige.ServiceDefaults", Layer.Hosting),

            new ("BienOblige.FakeDomain.Application", Layer.Application)
        };

    public static readonly ArchUnitNET.Domain.Architecture Architecture =
        new ArchLoader().LoadAssemblies(
            (Ruleset.Assemblies
                .Select(n => Assembly.Load(n.Key))).ToArray())
        .Build();

    public IArchRule AllRules
    {
        get
        {
            var result = _rules.First();
            foreach (var rule in _rules.Skip(1))
                result.And(rule);
            return result;
        }
    }

    public Ruleset()
    {
        var architecture = Ruleset.Architecture;

        var layers = System.Enum.GetValues(typeof(Layer));
        var layerConjunctions = new List<GivenTypesConjunctionWithDescription>();

        foreach (var layer in layers)
        {
            var layerAssemblies = Assemblies
                .Where(n => n.Value == (Layer)layer);

            foreach (var layerAssembly in layerAssemblies)
            {
                layerConjunctions.Add(Types().That().ResideInAssembly(layerAssembly.Key)
                    .As(layerAssembly.Value.ToString()));
            }
        }

        _rules = new List<IArchRule>()
        {
            Layer.Domain.ShouldNotDependUpon(Layer.Application),
            Layer.Domain.ShouldNotDependUpon(Layer.Infrastructure),
            Layer.Domain.ShouldNotDependUpon(Layer.Interface),
            Layer.Domain.ShouldNotDependUpon(Layer.Hosting),

            Layer.Application.ShouldNotDependUpon(Layer.Infrastructure),
            Layer.Application.ShouldNotDependUpon(Layer.Interface),
            Layer.Application.ShouldNotDependUpon(Layer.Hosting),

            Layer.Infrastructure.ShouldNotDependUpon(Layer.Interface),
            Layer.Infrastructure.ShouldNotDependUpon(Layer.Hosting),

            Layer.Interface.ShouldNotDependUpon(Layer.Hosting)
        };

    }

    public void CheckAllRules() => AllRules.Check(Ruleset.Architecture);

    public IEnumerator<IArchRule> GetEnumerator() => _rules.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _rules.GetEnumerator();
}
