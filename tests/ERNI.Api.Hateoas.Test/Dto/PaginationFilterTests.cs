namespace ERNI.Api.Hateoas.Dto.Tests;

public class PaginationFilterTests
{
    [Fact()]
    public void ShouldHaveDefaultValuesOnPropertiesWhenCreatedWithParameterlessConstructor()
    {
        // arrange
        PaginationFilter paginationFilter;

        // act
        paginationFilter = new PaginationFilter();

        // assert
        using (new AssertionScope())
        { 
            paginationFilter.PageNumber.Should().Be(1);
            paginationFilter.PageSize.Should().Be(1);
        }
    }

    [Theory]
    [MemberData(nameof(DataGenerator))]
    public void ShouldHaveInitalizedPropertiesWhenCreatedWithParameterConstructor(int pageNumber, int pageSize, int expectedPageNumber, int expectedPageSize)
    {
        // arrange
        PaginationFilter paginationFilter;

        // act
        paginationFilter = new PaginationFilter(pageNumber, pageSize);

        // assert
        using (new AssertionScope())
        {
            paginationFilter.PageNumber.Should().Be(expectedPageNumber);
            paginationFilter.PageSize.Should().Be(expectedPageSize);
        }
    }

    public static IEnumerable<object[]> DataGenerator => new List<object[]>
    {
        new object[] { 1, 1, 1, 1},
        new object[] { -1, 1, 1, 1},
        new object[] { 1, -1, 1, 1},
        new object[] { -1, -1, 1, 1} 
    };
}