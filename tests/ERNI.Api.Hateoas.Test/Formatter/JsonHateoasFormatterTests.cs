using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Services;
using ERNI.Api.Hateoas.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace ERNI.Api.Hateoas.Formatter.Tests;

public class JsonHateoasFormatterTests
{
    [Fact()]
    public void JsonHateoasFormatterTest()
    {
        // arrange
        JsonHateoasFormatter formatter;

        // act
        formatter = new JsonHateoasFormatter();

        // assert
        formatter.SupportedMediaTypes.FirstOrDefault().Should().Be("application/json+hateoas");
    }

    [Theory]
    [MemberData(nameof(DataGenerator))]
    public async Task WriteResponseBodyAsyncTest(Type itemType, object item, object expectedResponse)
    {
        // arrange 
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        IServiceProvider serviceProvider = new ServiceCollection().AddSingleton<IDataShaper, DataShaper>()
                                                                  .AddSingleton<ILinkGenerator<StudentDto>, StudentLinkGenerator>()
                                                                  .BuildServiceProvider();
        httpContext.RequestServices = serviceProvider;
        var outputFormatterWriteContext = new OutputFormatterWriteContext(httpContext, writeFactory, itemType, item);
        var formatter = new JsonHateoasFormatter();

        // act
        await formatter.WriteResponseBodyAsync(outputFormatterWriteContext);

        // assert
        string body = string.Empty;
        httpContext.Response.Body.Position = 0;
        using (var reader = new StreamReader(httpContext.Response.Body))
        {
            body = await reader.ReadToEndAsync();
        }
       
        var output = JsonConvert.DeserializeObject(body, expectedResponse.GetType());
        output.Should().BeEquivalentTo(expectedResponse);
    }

    public static IEnumerable<object[]> DataGenerator()
    {
        var items = new List<StudentDto> { new StudentDto() { Id = Guid.Empty }, new StudentDto() { Id = Guid.Empty } };
        return new List<object[]>
        {
            new object[]
            {
                typeof(StudentDto),
                new StudentDto(){ Id = Guid.Empty },
                new Test.Helpers.ResponseDto
                {
                    Id = Guid.Empty,
                    Links = new List<Link>
                    {
                        new Link($"https://localhost/student/?id={Guid.Empty}", "self", "GET"),
                        new Link($"https://localhost/student/?id={Guid.Empty}", "delete_student", "DELETE"),
                        new Link($"https://localhost/student/?id={Guid.Empty}", "update_student", "PUT")
                    }
                }
            },
            new object[]
            {
                typeof(IEnumerable<StudentDto>),
                new List<StudentDto>() { new StudentDto() { Id = Guid.Empty } } ,
                new LinkCollectionWrapperOfResponseDto()
                {
                    Value = new List<Test.Helpers.ResponseDto>
                    {
                        new Test.Helpers.ResponseDto()
                        {
                            Links = new List<Link>
                            {
                                new Link($"https://localhost/student/?id={Guid.Empty}", "self", "GET"),
                                new Link($"https://localhost/student/?id={Guid.Empty}", "delete_student", "DELETE"),
                                new Link($"https://localhost/student/?id={Guid.Empty}", "update_student", "PUT")
                            }
                        }
                    },
                    Links = new List<Link>
                    {
                        new Link("https://localhost/student/", "self", "GET"),
                        new Link("https://localhost/student/", "add", "POST")
                    }
                }
            },
            new object[]
            {
                typeof(IEnumerable<StudentDto>),
                PagedList<StudentDto>.ToPagedList(items, 1, 1),
                new LinkCollectionWrapperOfResponseDto()
                {
                    Value = new List<Test.Helpers.ResponseDto>
                    {                        
                        new Test.Helpers.ResponseDto()
                        {
                            Links = new List<Link>
                            {
                                new Link($"https://localhost/student/?id={Guid.Empty}", "self", "GET"),
                                new Link($"https://localhost/student/?id={Guid.Empty}", "delete_student", "DELETE"),
                                new Link($"https://localhost/student/?id={Guid.Empty}", "update_student", "PUT")
                            }
                        }
                    },
                    Links = new List<Link>
                    {
                        new Link("https://localhost/student/", "self", "GET"),
                        new Link("https://localhost/student/", "add", "POST"),
                        new Link($"https://localhost/student/?pageNumber=2&pageSize=1", "next", "GET"),
                        new Link($"https://localhost/student/?pageNumber=2&pageSize=1", "last", "GET")
                    }
                }
            },
            new object[]
            {
                typeof(IEnumerable<StudentDto>),
               PagedList<StudentDto>.ToPagedList(items, 2, 1) ,
                new LinkCollectionWrapperOfResponseDto()
                {
                    Value = new List<Test.Helpers.ResponseDto>
                    {
                        new Test.Helpers.ResponseDto()
                        {
                            Links = new List<Link>
                            {
                                new Link($"https://localhost/student/?id={Guid.Empty}", "self", "GET"),
                                new Link($"https://localhost/student/?id={Guid.Empty}", "delete_student", "DELETE"),
                                new Link($"https://localhost/student/?id={Guid.Empty}", "update_student", "PUT")
                            }
                        }
                    },
                    Links = new List<Link>
                    {
                        new Link("https://localhost/student/", "self", "GET"),
                        new Link("https://localhost/student/", "add", "POST"),
                        new Link($"https://localhost/student/?pageNumber=1&pageSize=1", "previous", "GET"),
                        new Link($"https://localhost/student/?pageNumber=1&pageSize=1", "first", "GET")
                    }
                }
            }
        };
    }

    private TextWriter writeFactory(Stream arg1, Encoding arg2)
    {
        return new StringWriter();
    }
}
