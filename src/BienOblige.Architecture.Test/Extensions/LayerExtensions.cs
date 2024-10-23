using ArchUnitNET.Fluent;
using BienOblige.Architecture.Test.Enumerations;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BienOblige.Architecture.Test.Extensions;

internal static class LayerExtensions
{
    internal static IArchRule ShouldNotDependUpon(this Layer layer, Layer otherLayer) =>
        Types().That().Are(layer.ToString()).Should().NotDependOnAny(otherLayer.ToString());
}
