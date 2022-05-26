using ERNI.Api.Hateoas.Dto;

namespace ERNI.Api.Hateoas.Services;

public interface ILinkGenerator<T> : ILinkGenerator
{
}

public interface ILinkGenerator
{
    public IEnumerable<Link> GetLinks<T>(T item) where T : new();
}
