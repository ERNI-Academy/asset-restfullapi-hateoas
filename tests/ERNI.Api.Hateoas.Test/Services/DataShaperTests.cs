using ERNI.Api.Hateoas.Test.Helpers;

namespace ERNI.Api.Hateoas.Services.Tests;

public class DataShaperTests
{
    [Fact()]
    public void ShapeDataTest()
    {
        // arrange
        var dataShaper = new DataShaper();
        var item = new StudentDto();
        var fields = "Name";

        // act
        var response = dataShaper.ShapeData(item, "Name");

        // assert
        using (new AssertionScope())
        { 
            response.Count().Should().Be(1);
            response.First().Keys.Count().Should().Be(1);
            response.First().ContainsKey(fields);
        }
    }
}