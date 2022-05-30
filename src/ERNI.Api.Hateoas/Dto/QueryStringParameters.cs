namespace ERNI.Api.Hateoas.Dto;

public class QueryStringParameters
{
    private int _pageSize = 1;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get { return _pageNumber; }
        set { _pageNumber = value < 0 ? 1 : value; }
    }


    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value <= 0 ? 1 : value;
        }
    }

    public string OrderBy { get; set; } = string.Empty;

    public string Fields { get; set; } = string.Empty;
}