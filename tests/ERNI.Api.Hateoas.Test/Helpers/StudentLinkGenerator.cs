
using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Services;

namespace ERNI.Api.Hateoas.Test.Helpers;

public class StudentLinkGenerator : ILinkGenerator<StudentDto>
{

    public StudentLinkGenerator()
    {
    }

    public IEnumerable<Link> GetLinks<T>(T item) where T : new()
    {
        if (item is null)
        {
            return null;
        }

        if (item.GetType() == typeof(StudentDto))
        {
            return GetLinks(item as StudentDto);
        }

        if (item.GetType() == typeof(PagedList<StudentDto>))
        {
            return GetLinks(item as PagedList<StudentDto>);
        }

        if (item.GetType().GetInterfaces().Any(i => i == typeof(IEnumerable<StudentDto>)))
        {
            return GetLinks(item as IEnumerable<StudentDto>);
        }

        return null;
    }

    private IEnumerable<Link> GetLinks(PagedList<StudentDto> items)
    {
        var links = new List<Link>
        {
            new Link("https://localhost/student/", "self", "GET")
        };
        if (items.HasNext)
        {
            links.Add(new Link($"https://localhost/student/?pageNumber={items.CurrentPage + 1}&pageSize={items.PageSize}", "next", "GET"));
        }

        if (items.HasPrevious)
        {
            links.Add(new Link($"https://localhost/student/?pageNumber={items.CurrentPage - 1}&pageSize={items.PageSize}", "previous", "GET"));
        }

        if (items.CurrentPage != items.TotalPages)
        {
            links.Add(new Link($"https://localhost/student/?pageNumber={items.TotalPages}&pageSize={items.PageSize}", "last", "GET"));
        }

        if (items.CurrentPage != 1)
        {

            links.Add(new Link($"https://localhost/student/?pageNumber=1&pageSize={items.PageSize}", "first", "GET"));
        }

        links.Add(new Link("https://localhost/student/", "add", "POST"));

        return links;
    }

    private IEnumerable<Link> GetLinks(IEnumerable<StudentDto> items)
    {
        return new List<Link>
        {
            new Link("https://localhost/student/", "self", "GET"),
            new Link("https://localhost/student/", "add", "POST")
        };
    }

    private IEnumerable<Link> GetLinks(StudentDto item)
    {
        var links = new List<Link>();

        links.AddRange(new List<Link>()
        {
                new Link($"https://localhost/student/?id={item.Id}", "self", "GET"),
                new Link($"https://localhost/student/?id={item.Id}", "delete_student", "DELETE"),
                new Link($"https://localhost/student/?id={item.Id}", "update_student", "PUT")
        });

        return links;
    }
}