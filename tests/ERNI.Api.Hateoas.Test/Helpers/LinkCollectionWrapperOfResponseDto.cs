using ERNI.Api.Hateoas.Dto;

namespace ERNI.Api.Hateoas.Test.Helpers;

public class LinkCollectionWrapperOfResponseDto
{
    public List<ResponseDto> Value { get; set; }
    public List<Link> Links { get; set; }
}