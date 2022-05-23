using HateoasApi.Domain;
using HateoasApi.Domain.Dto;
using HateoasApi.Domain.QueryParameters;
using HateoasApi.Services.Helpers;

namespace ERNI.Api.Hateoas.Sample.Services
{
    public interface IStudentsService
    {
        public PagedList<StudentDto> GetStudents(StudentParameters studentParameters);

        public StudentDto GetStudentById(Guid id);

        public StudentDto CreateStudent(StudentToCreateUpdateDto studentToCreateDto);

        public void DeleteStudent(Guid id);

        public void UpdateStudent(Guid id, StudentToCreateUpdateDto studentToUpdateDto);
    }
}