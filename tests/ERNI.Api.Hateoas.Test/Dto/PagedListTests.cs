namespace ERNI.Api.Hateoas.Dto.Tests;

public class PagedListTests
{
    [Fact()]
    public void ShouldHaveNullPropertiesWhenCreatedWithParameterlessConstructor()
    {
        // arrange
        PagedList<int> pagedList;

        // act 
        pagedList = new PagedList<int>();

        // assert
        using (new AssertionScope())
        {
            pagedList.CurrentPage.Should().Be(1);
            pagedList.TotalPages.Should().Be(default);
            pagedList.PageSize.Should().Be(1);
            pagedList.TotalCount.Should().Be(default);
            pagedList.HasPrevious.Should().BeFalse();
            pagedList.HasNext.Should().BeFalse();
            pagedList.Select(x => x).Should().BeEmpty();
        }
    }

    [Theory]
    [MemberData(nameof(DataGeneratorForConstructor))]
    public void ShouldHaveInitalizedPropertiesWhenCreatedWithParameterConstructor(List<int> items, int count, int pageNumber, int pageSize, bool hasPrevious, bool hasNext)
    {
        // arrange
        PagedList<int> pagedList;
        // act
        pagedList = new PagedList<int>(items, count, pageNumber, pageSize);

        // assert
        using (new AssertionScope())
        {
            pagedList.Select(x => x).Should().BeEquivalentTo(items);
            pagedList.CurrentPage.Should().Be(pageNumber);
            pagedList.PageSize.Should().Be(pageSize);
            pagedList.TotalPages.Should().Be((int)Math.Ceiling(count / (double)pageSize));
            pagedList.TotalCount.Should().Be(count);
            pagedList.HasNext.Should().Be(hasNext);
            pagedList.HasPrevious.Should().Be(hasPrevious);
        }
    }

    [Theory]
    [MemberData(nameof(DataGeneratorForConverter))]
    public void ShouldConvertAnIEnumerableToPagedListWhenConverToPagedListCalled(List<int> items, int count, int pageNumber, int pageSize, bool hasPrevious, bool hasNext)
    {
        // arrange
        PagedList<int> pagedList;

        // act
        pagedList = PagedList<int>.ToPagedList(items, pageNumber, pageSize);

        // assert
        using (new AssertionScope())
        {
            pagedList.Should().BeEquivalentTo(items.GetRange(pageNumber-1, pageSize));
            pagedList.CurrentPage.Should().Be(pageNumber);
            pagedList.PageSize.Should().Be(pageSize);
            pagedList.TotalPages.Should().Be((int)Math.Ceiling(count / (double)pageSize));
            pagedList.TotalCount.Should().Be(count);
            pagedList.HasNext.Should().Be(hasNext);
            pagedList.HasPrevious.Should().Be(hasPrevious);
        }
    }

    public static IEnumerable<object[]> DataGeneratorForConstructor()
    {
        List<int> items = GenerateList();
        return new List<object[]>
        {
            new object[] { new List<int>() { items.First() }, items.Count, 1, 1, false, true},
            new object[] { new List<int>() { items[2] }, items.Count, 2, 1, true, true},
            new object[] { new List<int>() { items.Last() }, items.Count, items.Count, 1, true, false},
        };
    }

    public static IEnumerable<object[]> DataGeneratorForConverter()
    {
        List<int> items = GenerateList();
        return new List<object[]>
        {
            new object[] { items, items.Count, 1, 1, false, true},
            new object[] { items, items.Count, 2, 1, true, true},
            new object[] { items, items.Count, items.Count, 1, true, false},
        };
    }

    private static List<int> GenerateList() => Enumerable.Range(1, 9).ToList();

}