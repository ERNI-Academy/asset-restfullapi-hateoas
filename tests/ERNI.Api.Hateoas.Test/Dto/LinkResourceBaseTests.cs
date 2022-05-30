namespace ERNI.Api.Hateoas.Dto.Tests;

public class LinkResourceBaseTests
{
    [Fact()]
    public void ShouldHaveEmptyPropertiesWhenCreated()
    {
        // arrange
        LinkResourceBase linkResourceBase;
       
        // act
        linkResourceBase = new LinkResourceBase();

        // assert
        linkResourceBase.Links.Should().NotBeNull().And.BeEmpty();
    }
}