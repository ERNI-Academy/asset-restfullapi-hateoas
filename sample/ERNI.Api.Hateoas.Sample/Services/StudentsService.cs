using AutoMapper;
using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Sample.Dto;
using ERNI.Api.Hateoas.Sample.Entities;
using ERNI.Api.Hateoas.Sample.QueryParameters;
using ERNI.Api.Hateoas.Services;

namespace ERNI.Api.Hateoas.Sample.Services;

public class StudentsService : IStudentsService
{
    //private UnitOfWork unitOfWork = new UnitOfWork();
    private readonly ISortHelper<Student> _sortHelper;
    private readonly IMapper _mapper;

    public StudentsService(ISortHelper<Student> sortHelper, IMapper mapper)
    {
        //unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _sortHelper = sortHelper;
        _mapper = mapper;
    }

    public PagedList<StudentDto> GetStudents(StudentParameters studentParameters)
    {
        //var students = this.unitOfWork.StudentRepository.Get();
        var sortedOwners = _sortHelper.ApplySort(GetRandomStudents().AsQueryable(), studentParameters.OrderBy);

        var shapedOwners = _mapper.Map<IEnumerable<StudentDto>>(sortedOwners);

        return PagedList<StudentDto>.ToPagedList(shapedOwners,
            studentParameters.PageNumber,
            studentParameters.PageSize);
    }

    public StudentDto GetStudentById(Guid id)
    {
        //var student = this.unitOfWork.StudentRepository.GetByID(id);
        var student = GetRandomStudents().FirstOrDefault(i => i.Id == id);
        return _mapper.Map<StudentDto>(student);
    }

    public StudentDto CreateStudent(StudentToCreateUpdateDto studentToCreateDto)
    {
        var student = _mapper.Map<Student>(studentToCreateDto);
        student.Id = Guid.NewGuid();

        //this.unitOfWork.StudentRepository.Insert(student);
        //this.unitOfWork.Save();

        return _mapper.Map<StudentDto>(student);
    }

    public void UpdateStudent(Guid id, StudentToCreateUpdateDto studentToUpdateDto)
    {
        var student = _mapper.Map<Student>(studentToUpdateDto);
        student.Id = id;

        //this.unitOfWork.StudentRepository.Update(student);
        //this.unitOfWork.Save();
    }

    public void DeleteStudent(Guid id)
    {
        //this.unitOfWork.StudentRepository.Delete(id);
        //this.unitOfWork.Save();
    }

    private IEnumerable<Student> GetRandomStudents()
    {
        return new List<Student>
        {
            new Student
            {
                Address = "Calle 1",
                DateOfBirth = new DateTime(2022, 7, 15, 3, 15, 0),
                Name = "Pere",
                Id = new Guid("431DA10A-627A-415A-8A57-519494BD265F")
            },
            new Student
            {
                Address = "Calle 2",
                DateOfBirth = new DateTime(2022, 11, 21, 4, 1, 0),
                Name = "Maria",
                Id = new Guid("1CFCB65C-1705-494D-954B-13A33C8EFB3D")
            }
        };
    }
}
