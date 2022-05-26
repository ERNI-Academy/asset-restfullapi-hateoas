namespace ERNI.Api.Hateoas.Dto;

public class LinkResourceBase
{
    public LinkResourceBase()
    {

    }

    public List<Link> Links { get; set; } = new List<Link>();
}
