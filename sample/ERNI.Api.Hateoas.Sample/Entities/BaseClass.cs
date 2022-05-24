using System.ComponentModel.DataAnnotations;

namespace ERNI.Api.Hateoas.Sample
{
    public class BaseClass
    {
        [Key]
        public Guid Id { get; set; }
    }
}
