using ERNI.Api.Hateoas.Services;
using ERNI.Api.Hateoas.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using ERNI.Api.Hateoas.Dto;
using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ERNI.Api.Hateoas.Formatter.Tests;

public class XmlHateoasFormatterTests
{
    [Fact()]
    public void XmlHateoasFormatterTest()
    {
        // arrange
        XmlHateoasFormatter formatter;

        // act
        formatter = new XmlHateoasFormatter();

        // assert
        formatter.SupportedMediaTypes.FirstOrDefault().Should().Be("application/xml+hateoas");
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
        var formatter = new XmlHateoasFormatter();
        var serializer = new XmlSerializer(expectedResponse.GetType());

        // act
        await formatter.WriteResponseBodyAsync(outputFormatterWriteContext);

        // assert
        string body = string.Empty;
        httpContext.Response.Body.Position = 0;
        using (var reader = new StreamReader(httpContext.Response.Body))
        {
            body = await reader.ReadToEndAsync();
        }

        object output;
        using (TextReader reader = new StringReader(body))
        {
            output = serializer.Deserialize(reader);
        }

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
