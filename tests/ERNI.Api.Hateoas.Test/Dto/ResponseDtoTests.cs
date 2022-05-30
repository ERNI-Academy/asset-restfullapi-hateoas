using System.Dynamic;
using System.Text;
using System.Xml;

namespace ERNI.Api.Hateoas.Dto.Tests;

public class ResponseDtoTests
{
    [Fact()]
    public void TryGetMemberTest()
    {
        // arrange
        ResponseDto response = new ResponseDto();
        response.TrySetMember(new SetMemberBinderForTesting("Name", false), "Name");
        var expectedResponse = true;
        var expectedValue = "Name";
        object outputValue;

        // act
        var responseResult = response.TryGetMember(new GetMemberBinderForTesting("Name", false), out outputValue);

        // assert
        using (new AssertionScope())
        {
            responseResult.Should().Be(expectedResponse);
            outputValue.Should().Be(expectedValue);
        }
    }

    [Fact()]
    public void TrySetMemberTest()
    {
        // arrange
        ResponseDto response = new ResponseDto();
        var expectedResponse = true;

        // act
        var responseResult = response.TrySetMember(new SetMemberBinderForTesting("Name", false), "Name");

        // assert
        responseResult.Should().Be(expectedResponse);
    }

    [Fact()]
    public void GetSchemaTest()
    {
        // arrange
        ResponseDto response = new ResponseDto();

        // act
        Action action = () => response.GetSchema();

        // assert

        action.Should().Throw<NotImplementedException>();
    }

    [Fact()]
    public void ReadXmlTest()
    {
        // arrange
        var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><EntityWithLinks><value type=\"System.Int32\">1</value></EntityWithLinks>";
        XmlReader xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
        var responseDto = new ResponseDto();

        // act
        responseDto.ReadXml(xmlReader);

        // assert
        responseDto["value"].Should().Be(1);
    }

    [Fact()]
    public void WriteXmlTest()
    {
        // arrange
        var stringBuilder = new StringBuilder();
        var xmlWriter = XmlWriter.Create(stringBuilder);
        ResponseDto response = new ResponseDto();
        response.TrySetMember(new SetMemberBinderForTesting("Name", false), "Name");
        var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Name>Name</Name>";

        // act
        response.WriteXml(xmlWriter);
        xmlWriter.Flush();

        // assert
        stringBuilder.ToString().Should().Be(expectedXml);
    }

    [Fact()]
    public void AddTestWithKeyAndValue()
    {
        // arrange
        ResponseDto response = new ResponseDto();

        // act
        response.Add(nameof(response), nameof(response));

        // assert
        response[nameof(response)].Should().Be(nameof(response));
    }

    [Fact()]
    public void AddTestWithKeyValuePair()
    {
        // arrange
        ResponseDto response = new ResponseDto();

        // act
        response.Add(new KeyValuePair<string, object>(nameof(response), nameof(response)));

        // assert
        response[nameof(response)].Should().Be(nameof(response));
    }

    [Theory]
    [MemberData(nameof(DataGeneratorContainsKeyTest))]
    public void ContainsKeyTest(string property, string value, string expectedProperty, bool expectedPropertyExists)
    {
        // arrange
        var response = new ResponseDto();
        response.Add(property, value);

        // act
        var exists = response.ContainsKey(expectedProperty);

        // assert
        exists.Should().Be(expectedPropertyExists);
    }

    [Theory]
    [MemberData(nameof(DataGeneratorContainsKeyTest))]
    public void ContainsTest(string property, string value, string expectedProperty, bool expectedPropertyExists)
    {
        // arrange
        var response = new ResponseDto();
        response.Add(property, value);

        // act
        var exists = response.Contains(new KeyValuePair<string, object>(expectedProperty, value));

        // assert
        exists.Should().Be(expectedPropertyExists);
    }

    [Fact()]
    public void RemoveTest()
    {
        // arrange
        var response = new ResponseDto();
        response.Add(nameof(response), nameof(response));

        // act
        var exists = response.Remove(nameof(response));

        // assert
        using (new AssertionScope())
        {
            response.ContainsKey(nameof(response)).Should().BeFalse();
            exists.Should().BeTrue();
        }
    }

    [Fact()]
    public void RemoveTest1()
    {
        // arrange
        var response = new ResponseDto();
        response.Add(nameof(response), nameof(response));

        // act
        var exists = response.Remove(new KeyValuePair<string, object>(nameof(response), nameof(response)));

        // assert
        using (new AssertionScope())
        {
            response.ContainsKey(nameof(response)).Should().BeFalse();
            exists.Should().BeTrue();
        }
    }

    [Theory]
    [MemberData(nameof(DataGeneratorTryGetValueTest))]
    public void TryGetValueTest(string property, string value, string expectedProperty, object expectedValue, bool expectedCouldGet)
    {
        // arrange
        var response = new ResponseDto();
        response.Add(property, value);

        // act
        var couldGet = response.TryGetValue(expectedProperty, out object outValue);

        //assert
        using (new AssertionScope())
        {
            couldGet.Should().Be(expectedCouldGet);
            outValue.Should().Be(expectedValue);
        }
    }

    [Fact()]
    public void ClearTest()
    {
        // arrange
        var response = new ResponseDto();
        response.Add(nameof(response), nameof(response));
        response.Add("Name", "Name");

        // act
        response.Clear();

        // assert
        response.Keys.Should().BeEmpty();
    }

    [Fact()]
    public void CopyToTest()
    {
        // arrange
        var response = new ResponseDto();
        response.Add(nameof(response), nameof(response));
        response.Add("Name", "Name");

        KeyValuePair<string, object>[] responseArray = new KeyValuePair<string, object>[2];
        var expectedResponseArray = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>(nameof(response), nameof(response)),
            new KeyValuePair<string, object>("Name", "Name")
        };

        // act
        response.CopyTo(responseArray, 0);

        // assert
        responseArray.Should().BeEquivalentTo(expectedResponseArray);

    }

    public static IEnumerable<object[]> DataGeneratorContainsKeyTest()
    {
        return new List<object[]>
        {
            new object[] { "Name", "Name", "Name", true},
            new object[] { "Name", "Name", "Name1", false}
        };
    }
    public static IEnumerable<object?[]> DataGeneratorTryGetValueTest()
    {
        return new List<object?[]>
        {
            new object?[] { "Name", "Name", "Name", "Name", true},
            new object?[] { "Name", "Name", "Name1", null, false}
        };
    }



    private class SetMemberBinderForTesting : SetMemberBinder
    {
        public SetMemberBinderForTesting(string name, bool ignoreCase) : base(name, ignoreCase)
        {
        }

        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject? errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }

    private class GetMemberBinderForTesting : GetMemberBinder
    {
        public GetMemberBinderForTesting(string name, bool ignoreCase) : base(name, ignoreCase)
        {
        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject? errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}