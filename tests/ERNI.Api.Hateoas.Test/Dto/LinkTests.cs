namespace ERNI.Api.Hateoas.Dto.Tests;

public class LinkTests
{
    [Fact()]
    public void ShouldHaveNullPropertiesWhenCreatedWithParameterlessConstructor()
    {
        // arrange
        Link link;
        // act
        link = new Link();
        // assert
        using (new AssertionScope())
        {
            link.Href.Should().BeNullOrEmpty();
            link.Rel.Should().BeNullOrEmpty();
            link.Method.Should().BeNullOrEmpty();
        }
    }

    [Theory]
    [MemberData(nameof(DataGenerator))]
    public void ShouldHaveInitalizedPropertiesWhenCreatedWithParameterConstructor(string href, string rel, string method)
    {
        // arrange
        Link link;
        // act
        link = new Link(href, rel, method);
        // assert
        using (new AssertionScope())
        {
            link.Href.Should().Be(href);
            link.Rel.Should().Be(rel);
            link.Method.Should().Be(method);
        }
    }

    public static IEnumerable<object[]> DataGenerator => new List<object[]>
    {
        new object[] { "test", string.Empty, string.Empty},
        new object[] { string.Empty, "test", string.Empty},
        new object[] { string.Empty, string.Empty, "test"},

        new object[] { "test", "test", string.Empty},
        new object[] { string.Empty, "test", "test"},
        new object[] { "test", string.Empty, "test" },

        new object[] { "test", "test", "test"},
    };
}