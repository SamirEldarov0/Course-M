using AutoMapper;
using Lesson21_API_.Api.Manage.Dtos.CategoryDtos;
using Lesson21_API_.Api.Manage.Dtos.CourseDtos;
using Lesson21_API_.Data;
using Lesson21_API_.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson21_API_.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin,SuperAdmin")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CoursesController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int page=1)
        {
            var courses = await _context.Courses.Include(x=>x.Category).Skip((page - 1) * 10).Take(10).ToListAsync();
            CourseListDtos courseListDtos = new CourseListDtos()
            {
                Data =_mapper.Map<List<CourseListItemDto>>(courses),
                TotalPage= (int)Math.Ceiling(_context.Courses.Count() / 10d)
            };
            //return Ok(new { data = courses,totalPage = totalPage });
            return Ok(courseListDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Course course = await _context.Courses.Include(x=>x.CourseTags).ThenInclude(x=>x.Tag).FirstOrDefaultAsync(x => x.Id == id);
            if (course==null)
            {
                return NotFound();
            }
            CourseDetailedDto courseDetailedDto = _mapper.Map<CourseDetailedDto>(course);
            courseDetailedDto.Tags = course.CourseTags.Select(x => new TagInCourseDetailedDto { TagId = x.TagId, Name = x.Tag.Name }).ToList();

            //courseDetailedDto.Tags = new List<TagInCourseDetailedDto>();
            //foreach (var item in course.CourseTags)
            //{
            //    TagInCourseDetailedDto tagInCourseDetailedDto = new TagInCourseDetailedDto()
            //    {
            //        Id = item.TagId,
            //        Name = item.Tag.Name
            //    };
            //    courseDetailedDto.Tags.Add(tagInCourseDetailedDto);
            //}
            return Ok(courseDetailedDto);
            //return StatusCode(200,courseDetailedDto);
        }


        [HttpPost("")]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            if (!_context.Categories.Any(x => x.Id == courseCreateDto.CategoryId&&!x.IsDeleted)) return NotFound(); /*StatusCode(404);*/
            //silinen bir categoridirse not found olur
            //Eger course yaradanda elave olunan categoryId yoxdursa categorilerde error!!!!!!!!!!!!!!!!!!
            //NotFound("Category not found");

            Course course =_mapper.Map<Course>(courseCreateDto);
            //course.CourseTags = new List<CourseTag>();
            //if (courseCreateDto.TagIds!=null&& courseCreateDto.TagIds.Count>0)
            //{
            //    course.CourseTags = courseCreateDto.TagIds.Select(x => new CourseTag { TagId = x }).ToList();

            //    //foreach (var item in courseCreateDto.TagIds)
            //    //{

            //    //    CourseTag courseTag = new CourseTag()
            //    //    {
            //    //        TagId = item,
            //    //        //Course = course
            //    //    };
            //    //    course.CourseTags.Add(courseTag);
            //    //}
            //}
           
            await _context.Courses.AddAsync(course);

            await _context.SaveChangesAsync();

            return StatusCode(201, new { Id = course.Id });//200-de qaytara bilerik ---?,201 icinde course qaytara bilerem amma hamsini eyni mentiqde et

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,CourseCreateDto updateDto)//Update olunacaq entitinin adi  ve update ucun gonderdiyim model
        {
            Course course = await _context.Courses.Include(x=>x.CourseTags).FirstOrDefaultAsync(x => x.Id == id);
            if (course==null)
            {
                return NotFound();
                return StatusCode(404);
            }

            if (!_context.Categories.Any(x=>x.Id==updateDto.CategoryId&&!x.IsDeleted))
            {
                //Course icinde 5 dene foreign key gelir
                return NotFound();//Iki dene not found var icinde mesaj gondre bilerem amma duzgun deil,her 400un icinde custom obyekt qaytarsaq
                //icinde status kodu (4001,4002,...),mesaji,desc  bele problemi hell ede bilerik
                //yada ele bir obyekt qaytaririqki icinde xetali propertini qaytaririq  ,properti listi icinde xetali prop adlari
            }
            //Niye mapp istf elemedin cunki db-dan  yuxaridaki course-u istf etmir yeni obj yaradir,db-dan track olunan obj vermir yeni obj yaradir
            //

            // course = _mapper.Map<Course>(updateDto);//Bu halda price deyismir  burada map edende yeni bir course obyekti yaradir
            // Course course  database-den goturduyum deyer budur men birbasa bunun uzerinde deyisiklik edende entiti onu izleye bilir
            //ve burada track olunan value basqa obyektle deyisir
            course.CategoryId = updateDto.CategoryId;
            course.Name = updateDto.Name;
            course.Price = updateDto.Price;
            course.Desc = updateDto.Desc;
            course.StartDate = updateDto.StartDate;
            
            //var tags = await _context.Tags.Select(x => updateDto.TagIds).ToListAsync();
            //var newcoursetags = updateDto.TagIds;

            //if (newcoursetags!=null&&newcoursetags.Count>0)
            //{
            //    foreach (var item in newcoursetags)
            //    {
            //        if (course.CourseTags)
            //        {

            //        }
            //    }
            //}

            course.CourseTags.RemoveRange(0, course.CourseTags.Count);
            if (updateDto.TagIds != null&&updateDto.TagIds.Count>0)
            {
                course.CourseTags = updateDto.TagIds.Select(x => new CourseTag { TagId = x }).ToList();
            }


            await _context.SaveChangesAsync();


            return NoContent();


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Course course =await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course==null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            
            return StatusCode(204);
        }

    }
}
