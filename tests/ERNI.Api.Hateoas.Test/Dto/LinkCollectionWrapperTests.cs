namespace ERNI.Api.Hateoas.Dto.Tests;

public class LinkCollectionWrapperTests
{
    [Fact()]
    public void ShouldHaveEmptyPropertiesWhenCreatedWithParameterlessConstructor()
    {
        // arrange
        LinkCollectionWrapper<int> linkCollectionWrapper;

        // act
        linkCollectionWrapper = new LinkCollectionWrapper<int>();

        // assert
        using (new AssertionScope())
        {
            linkCollectionWrapper.Value.Should().NotBeNull().And.BeEmpty();
            linkCollectionWrapper.Links.Should().NotBeNull().And.BeEmpty();
        }
    }

    [Fact()]
    public void ShouldHaveInitalizedPropertiesWhenCreatedWithParameterConstructor()
    {
        // arrange
        LinkCollectionWrapper<int> linkCollectionWrapper;
        var items = Enumerable.Range(1, 9).ToList();

        // act        
        linkCollectionWrapper = new LinkCollectionWrapper<int>(items);

        // assert
        using (new AssertionScope())
        {
            linkCollectionWrapper.Value.Should().NotBeNullOrEmpty();
            linkCollectionWrapper.Value.Should().BeEquivalentTo(items);
            linkCollectionWrapper.Links.Should().NotBeNull().And.BeEmpty();
        }
    }
}