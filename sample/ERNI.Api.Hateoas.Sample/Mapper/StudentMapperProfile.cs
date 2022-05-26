using AutoMapper;
using ERNI.Api.Hateoas.Sample.Dto;
using ERNI.Api.Hateoas.Sample.Entities;

namespace ERNI.Api.Hateoas.Sample.Mapper;

public class StudentMapperProfile : Profile
	{
		public StudentMapperProfile()
		{
			CreateMap<Student, StudentDto>().ReverseMap();
			CreateMap<Student, StudentToCreateUpdateDto>().ReverseMap();
		}
	}
