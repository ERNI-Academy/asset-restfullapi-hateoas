using ERNI.Api.Hateoas.Dto;

namespace ERNI.Api.Hateoas.Services;

public interface IDataShaper
{
    IEnumerable<ResponseDto> ShapeData(object entity, string fieldsString);
}