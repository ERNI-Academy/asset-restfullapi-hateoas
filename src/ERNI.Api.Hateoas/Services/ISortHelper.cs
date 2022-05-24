namespace ERNI.Api.Hateoas.Services
{
    public interface ISortHelper<T>
	{
		IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
	}
}