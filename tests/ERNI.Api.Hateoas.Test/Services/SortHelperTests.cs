using ERNI.Api.Hateoas.Test.Helpers;

namespace ERNI.Api.Hateoas.Services.Tests;

public class SortHelperTests
{
    [Theory]
    [MemberData(nameof(DataGenerator))]
    public void ApplySortTest(List<StudentDto> items, string orderByQueryString, List<StudentDto> expectedItems)
    {
        // arrange
        var sortHelper = new SortHelper<StudentDto>();

        // act
        var result = sortHelper.ApplySort(items.AsQueryable(), orderByQueryString);

        // assert
        result.Should().BeEquivalentTo(expectedItems, opt => opt.WithStrictOrdering());
    }

    public static IEnumerable<object[]> DataGenerator()
    {var item1 = new StudentDto
        {
            Address = "Street 1",
            DateOfBirth = DateTime.Now.ToString(),
            Name = "Pere",
            Id = Guid.NewGuid()
        };
        var item2 = new StudentDto
        {
            Address = "Street 2",
            DateOfBirth = DateTime.Now.AddDays(1).ToString(),
            Name = "Maria",
            Id = Guid.NewGuid()
        };
        return new List<object[]>
        {
            new object[]
            {
                new List<StudentDto>{ item1, item2 },
                "name desc",
                new List<StudentDto>{ item1, item2 }
            },
            new object[]
            {
                new List<StudentDto>{ item1, item2 },
                "name asc",
                new List<StudentDto>{ item2, item1 }
            }
        };
    }
}