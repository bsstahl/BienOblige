using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.Api.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class ActionItemCollectionBuilder_Build_Should
{
    [Fact]
    public void ResultInAParentChildRelationshipBetweenActionItems()
    {
        var content = new CreateActionItemActivitiesBuilder()
            .CorrelationId(Guid.NewGuid())
            .ActivityType(Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Enumerations.ActorType.Application)
                .Name("MyTaskSystem"))
            .ActionItems(new ActionItemCollectionBuilder()
                .Add(new ActionItemBuilder()
                    .Name("Parent Action Item")
                    .Content("This is the content of the parent item", TestHelpers.DefaultMediaType)
                    .Children(new ActionItemCollectionBuilder()
                        .Add(new ActionItemBuilder()
                            .Name("Child Action Item")
                            .Content("This is the content of the child item", TestHelpers.DefaultMediaType)))))
            .Build();

        var actualParentActivity = content.Single(r => r.Object.AsActionItem().Parent is null);
        var actualParent = actualParentActivity.Object.AsActionItem();

        var actualChildActivity = content.Single(r => r.Object.AsActionItem().Parent is not null);
        var actualChild = actualChildActivity.Object.AsActionItem();

        // The child item's ParentId should be the parent item's Id
        Assert.Equal(actualChild.Parent, actualParent.Id.ToString());
    }
}
