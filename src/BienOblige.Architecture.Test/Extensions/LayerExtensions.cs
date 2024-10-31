using ArchUnitNET.Fluent;
using BienOblige.Architecture.Test.Enumerations;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BienOblige.Architecture.Test.Extensions;

internal static class LayerExtensions
{
    internal static IArchRule ShouldNotDependUpon(this Layer layer, Layer otherLayer) =>
        Types().That().HaveFullNameContaining(layer.ToString()).Should().NotDependOnAnyTypesThat().HaveFullNameContaining(otherLayer.ToString());

    //internal static IArchRule ShouldNotDependUpon(this Layer layer, Layer otherLayer) =>
    //    Types().That().ResideInNamespace(layer.ToString()).Should().NotDependOnAnyTypesThat()
    //        .ResideInNamespace(otherLayer.ToString());

    //internal static IArchRule ShouldNotDependUpon(this Layer layer, Layer otherLayer) =>
    //    Types().That().ResideInNamespace($"*.{layer}.*").Should().NotDependOnAnyTypesThat()
    //        .ResideInNamespace($"*.{otherLayer}.*");

}
