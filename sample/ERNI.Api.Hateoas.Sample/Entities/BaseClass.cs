using System.ComponentModel.DataAnnotations;

namespace ERNI.Api.Hateoas.Sample.Entities;

public class BaseClass
{
    [Key]
    public Guid Id { get; set; }
}
