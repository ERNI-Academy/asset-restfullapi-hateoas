using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ERNI.Api.Hateoas.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private IStudentsService _studentsService;

        public StudentController(ILogger<StudentsController> logger,
            IStudentsService studentsService)
        {
            _logger = logger;
            _studentsService = studentsService;
        }

        [HttpGet(Name = "GetStudents")]
        public IActionResult GetStudents([FromQuery] StudentParameters studentParameters)
        {
            var result = _studentsService.GetStudents(studentParameters);

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetStudentById")]
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetStudentById(Guid id, [FromQuery] StudentParameters studentParameters) => Ok(_studentsService.GetStudentById(id));

        [HttpPost(Name = "CreateStudent")]
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult CreateStudent(StudentToCreateUpdateDto studentToCreateDto)
        {
            var student = _studentsService.CreateStudent(studentToCreateDto);
            return CreatedAtRoute("GetStudentById", new { id = student.Id }, student);
        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult DeleteStudent(Guid id)
        {
            _studentsService.DeleteStudent(id);
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateStudent")]
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult UpdateStudent(Guid id, [FromBody] StudentToCreateUpdateDto studentToUpdate, [FromQuery] StudentParameters studentParameters)
        {
            _studentsService.UpdateStudent(id, studentToUpdate);
            return NoContent();
        }
    }
}