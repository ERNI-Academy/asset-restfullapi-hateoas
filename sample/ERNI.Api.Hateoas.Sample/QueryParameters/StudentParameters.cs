using ERNI.Api.Hateoas.Dto;

namespace ERNI.Api.Hateoas.Sample.QueryParameters
{
    public class StudentParameters : QueryStringParameters
	{
		public StudentParameters()
		{
			OrderBy = "name";
		}

		public uint MinYearOfBirth { get; set; }
		public uint MaxYearOfBirth { get; set; } = (uint)DateTime.Now.Year;

		public bool ValidYearRange => MaxYearOfBirth > MinYearOfBirth;

		public string Name { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;
	}
}
