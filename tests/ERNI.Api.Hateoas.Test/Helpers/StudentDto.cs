namespace ERNI.Api.Hateoas.Test.Helpers;
public class StudentDto
{
    public string Name { get; set; } = string.Empty;

    public string DateOfBirth { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public Guid Id { get; set; } = Guid.Empty;
}
