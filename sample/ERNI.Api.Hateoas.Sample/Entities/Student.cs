using System.ComponentModel.DataAnnotations;

namespace ERNI.Api.Hateoas.Sample.Entities
{
    public class Student : BaseClass
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100, ErrorMessage = "Address can not be loner then 100 characters")]
        public string Address { get; set; }
    }
}
