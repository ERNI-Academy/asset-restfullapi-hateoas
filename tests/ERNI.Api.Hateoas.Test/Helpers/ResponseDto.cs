using ERNI.Api.Hateoas.Dto;

namespace ERNI.Api.Hateoas.Test.Helpers;

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]

public class ResponseDto
{
    public string Name { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Guid Id { get; set; } = Guid.Empty;
    public List<Link> Links { get; set; }
}
