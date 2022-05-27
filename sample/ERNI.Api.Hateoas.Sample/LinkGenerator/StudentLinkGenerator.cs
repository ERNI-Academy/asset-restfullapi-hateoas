using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Sample.Dto;
using ERNI.Api.Hateoas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ERNI.Api.Hateoas.Sample.LinkGenerator;

internal class StudentLinkGenerator : ILinkGenerator<StudentDto>
{
    private readonly IUrlHelper _urlHelper;

    public StudentLinkGenerator(IUrlHelperFactory urlHelperFactory, IActionContextAccessor contextAccessor)
    {
        _urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
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

        if (item.GetType() == typeof(IEnumerable<StudentDto>))
        {
            return GetLinks();
        }

        return null;
    }

    private IEnumerable<Link> GetLinks(PagedList<StudentDto> items)
    {
        var links = new List<Link>
        {
            new Link(_urlHelper.Action("GetStudents", "Student", values: new { }), "self", "GET")
        };

        if (items.HasNext)
        {
            links.Add(new Link(_urlHelper.Action("GetStudents", "Student", values: new PaginationFilter { PageNumber = items.CurrentPage + 1, PageSize = items.PageSize }),
                           "next",
                           "GET"));
        }

        if (items.HasPrevious)
        {
            links.Add(new Link(_urlHelper.Action("GetStudents", "Student", values: new PaginationFilter { PageNumber = items.CurrentPage - 1, PageSize = items.PageSize }),
                           "previous",
                           "GET"));
        }

        if (items.CurrentPage != items.TotalPages)
        {
            links.Add(new Link(_urlHelper.Action("GetStudents", "Student", values: new PaginationFilter { PageNumber = items.TotalPages, PageSize = items.PageSize }),
                                          "last",
                                          "GET"));
        }

        if (items.CurrentPage != 1)
        {
            links.Add(new Link(_urlHelper.Action("GetStudents", "Student", values: new PaginationFilter { PageNumber = 1, PageSize = items.PageSize }),
                           "first",
                           "GET"));
        }

        links.Add(new Link(_urlHelper.Action("CreateStudent", "Student"), "add", "POST"));

        return links;
    }

    private IEnumerable<Link> GetLinks()
    {
        return new List<Link>
        {
            new Link(_urlHelper.Action("GetStudents", "Student", values: new { }), "self", "GET"),
            new Link(_urlHelper.Action("CreateStudent", "Student"), "add", "POST")
        };
    }

    private IEnumerable<Link> GetLinks(StudentDto item)
    {
        var links = new List<Link>();

        links.AddRange(new List<Link>() 
        {
                new Link(_urlHelper.Action("GetStudentById", "Student", values: new { item.Id }), "self", "GET"),
                new Link(_urlHelper.Action("DeleteStudent", "Student", values: new { item.Id }), "delete_student", "DELETE"),
                new Link(_urlHelper.Action("UpdateStudent", "Student", values: new { item.Id }), "update_student", "PUT")
        });

        return links;
    }
}
