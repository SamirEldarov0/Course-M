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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]//[HttpGet("{id}")]-de olar
        [Route("")]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            var categories = await _context.Categories.Include(x=>x.Courses).Where(x => !x.IsDeleted).Skip((page - 1) * 10).Take(10).ToListAsync();
            CategoryListDto categoryListDto = new CategoryListDto()
            {
                Data = _mapper.Map<List<CategoryListItemDto>>(categories)
                //await _context.Categories.Where(x=>!x.IsDeleted).Skip((page - 1) * 10).Take(10)
                //.Select(x => new CategoryListItemDto  //category-nin categoryListItemDto-a cevrilmesi
                //{
                //    Id = x.Id,
                //    Name = x.Name
                //}).ToListAsync()
                ,
                TotalPage = (int)Math.Ceiling(_context.Categories.Where(x=>!x.IsDeleted).Count() / 10d)
            };
            //var x = new { Name = "Samir", Surname = "Eldarov" };
            return Ok(categoryListDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = await _context.Categories.ToListAsync();

            List<CategoryItemDto> categoryItemDtos = _mapper.Map<List<CategoryItemDto>>(categories);

            return Ok(categoryItemDtos);
        }

        [HttpGet]//[HttpGet("{id}")]-de olar
        [Route("{id}")]//[Route("")] Bos buraxmaq olmaz cunki getall-da bosdu
        public async Task<IActionResult> Get(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (category == null) return StatusCode(404);//NotFound()

            CategoryDetailedDto categoryDetailedDto = _mapper.Map<CategoryDetailedDto>(category);
            //    new CategoryDetailedDto()
            //{
            //    Id=category.Id,
            //    Name = category.Name
            //};
            return Ok(categoryDetailedDto);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            if (_context.Categories.Any(x => x.Name.ToLower() == categoryCreateDto.Name.ToLower()&&!x.IsDeleted))
                //eger silinibse yeni cat-nin IsDeletedi truesa bunnan yarada bil
                return StatusCode(409);//conflict
                                       //return StatusCode(402);//Payment Required required yoxdursa
            
            //create emeliyyatunda tezden bele bir obyekt duzeltmek
            //eslinde burda etdiyim emeliyyat bir nov bir map emeliyyatidir
            //Bir tipdeki datani basqa tipe map edirik
            //Bu emeliyyatlari avtomatik ede bilmek ucun saytin icinde mapper toollari istf ede bilerik,custom ozumuzde yarada bilerik
            //auto mapper - mvc-de de ola biler
            //ve layihemde bir dene mapping profile yaradiram 
            //Profile clasi auto mapper icinde gelir

            Category category = _mapper.Map<Category>(categoryCreateDto);
            //category.IsDeleted = false;
            //new Category()
            //{
            //    IsDeleted = false,
            //    Name = categoryCreateDto.Name,
            //};
            await _context.Categories.AddAsync(category);
            _context.SaveChanges();
            return StatusCode(201,new { Id=category.Id }); 
            return StatusCode(201, category);//create edir ve obyekti geri qaytarir 
            //return Created(); icinde nese var sorun yaradabilir
            //return Ok();
        }

        [HttpPut("{id}")]
        //[FromForm] Eger yoxdursa raw json ile editle
        public async Task<IActionResult> Edit(int id,[FromForm]CategoryCreateDto categoryUpdateDto)//Eger reateden basqa inputlarimiz varsa Edit-de IsDeeleted istiyirikse yeni dto yaradiram
        {
            Category category =await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);//category IsDeleted=true-sa null olsun cunki silinmis cat uzerinde
            //emeliyyat aparmaq olmur                                                                                                  
            if (category==null)
            {
                return NotFound();
                //return StatusCode(404);
            }
            if (_context.Categories.Any(x=>x.Name.ToLower()==categoryUpdateDto.Name.ToLower() && x.Id!=id && !x.IsDeleted))//
            {
                return StatusCode(409);
            }
            category.Name = categoryUpdateDto.Name;
            _context.SaveChanges();
            return StatusCode(204);
            return NoContent();//Emeliyyat icra olundu geriye qaytarilasi hec bir sey yoxdur nocontent=204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);//IsDeleted false olanlar
            if (category == null)
            {
                return NotFound();
            }

            //_context.Categories.Remove(category);
            category.IsDeleted = true;
            _context.SaveChanges();
            return StatusCode(204);
        }
    }
}
