using AutoMapper;
using Lesson21_API_.Api.Manage.Dtos.TagDtos;
using Lesson21_API_.Data;
using Lesson21_API_.Data.Entities;
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
    public class TagController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TagController(AppDbContext context,IMapper mapper)
        {
            _context= context;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(TagCreateDto tagCreateDto)
        {
            if (await _context.Tags.AnyAsync(x=>x.Name.ToLower()==tagCreateDto.Name.ToLower()))
            {
                return StatusCode(409);//bu adda artiq tag var
            }
            Tag tag=_mapper.Map<Tag>(tagCreateDto);

            await _context.Tags.AddAsync(tag);
            _context.SaveChanges();
            return StatusCode(201, tag);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,[FromForm]TagUpdateDto tagUpdateDto)
        {
            Tag tag =await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag==null)
            {
                return NotFound();
            }
            if (await _context.Tags.AnyAsync(x=>x.Name.ToLower()==tagUpdateDto.Name.ToLower()&&x.Id!=id))
            {
                return StatusCode(409);
            }
            tag.Name=tagUpdateDto.Name;
            _context.SaveChanges();

            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Tag tag =await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag==null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return StatusCode(204);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {

            List<Tag> tags = _context.Tags.ToListAsync().Result;
            if (tags == null)
            {
                return StatusCode(404);
            }
            TagListDto tagListDto = new TagListDto()
            {
                Data = _mapper.Map<List<TagListItemDto>>(tags),
                TotalPage = (int)Math.Ceiling(_context.Tags.Count() / 3d)
            };

            return Ok(tagListDto);

        }


       
        [HttpGet("{id}")]
        public IActionResult Get(int id=1)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag==null)
            {
                return NotFound();
            }
            //TagListItemDto tagListItemDto = _mapper.Map<TagListItemDto>(tag);
            TagDetailedDto tagDetailedDto = _mapper.Map<TagDetailedDto>(tag);

            return Ok(tagDetailedDto);
        }

        [HttpGet("all")]
        public IActionResult All()
        {
            List<Tag> tags = _context.Tags.ToList();
            List<TagItemDto> tagItemDtos = _mapper.Map<List<TagItemDto>>(tags);

            return Ok(tagItemDtos);
        }


        

    }
}
