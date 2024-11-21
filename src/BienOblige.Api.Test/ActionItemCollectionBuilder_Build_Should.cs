using BienOblige.Api.Builders;
using System.Diagnostics.CodeAnalysis;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class ActionItemCollectionBuilder_Build_Should
{
    [Fact]
    public void ResultInAParentChildRelationshipBetweenActionItems()
    {
        var content = new ActivitiesCollectionBuilder()
            .Id(Guid.NewGuid())
            .ActivityType(Enumerations.ActivityType.Create)
            .Actor(new ActorBuilder()
                .Id(Guid.NewGuid())
                .ActorType(Enumerations.ActorType.Application)
                .Name("MyTaskSystem"))
            .ActionItems(new ActionItemCollectionBuilder()
                .Add(new ActionItemBuilder()
                    .Name("Parent Action Item")
                    .Content("This is the content of the parent item")
                    .Children(new ActionItemCollectionBuilder()
                        .Add(new ActionItemBuilder()
                            .Name("Child Action Item")
                            .Content("This is the content of the child item")))))
            .Build();

        // The child item's ParentId should be the parent item's Id
        var actualParent = content.Single(r => r.ActionItem.Parent is null).ActionItem;
        var actualChild = content.Single(r => r.ActionItem.Parent is not null).ActionItem;

        Assert.Equal(2, content.Count());
        Assert.Equal(actualChild.Parent, actualParent.Id);
    }
}
