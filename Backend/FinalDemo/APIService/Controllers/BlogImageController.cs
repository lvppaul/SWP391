﻿using AutoMapper;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Response;
using Domain.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.KCSAH.Repository;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogImageController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogImageController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogImageDTO>>> GetAllSync()
        {
            var blogImages = await _unitOfWork.BlogImageRepository.GetAllAsync();
            var blogImageDTOs = _mapper.Map<List<BlogImageDTO>>(blogImages);
            return Ok(blogImageDTOs);
        }

        [HttpGet("async/{id}")]
        public async Task<ActionResult<BlogImageDTO>> GetByIdAsync(int id)
        {
            var blogImage = await _unitOfWork.BlogImageRepository.GetByIdAsync(id);
            if (blogImage == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<BlogImageDTO>(blogImage);
            return result;
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<BlogImageDTO> GetById(int id)
        {
            var blogImage = _unitOfWork.BlogImageRepository.GetById(id);
            if (blogImage == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<BlogImageDTO>(blogImage);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlogImage([FromBody] BlogImageRequestDTO blogImagedto)
        {
            if (blogImagedto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var blogImageMap = _mapper.Map<BlogImage>(blogImagedto);
            var createResult = await _unitOfWork.BlogImageRepository.CreateAsync(blogImageMap);
            if (createResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            // Cập nhật lại giá trị ShopId cho shopdto từ shopMap
            var blogImageReturn = _mapper.Map<BlogImageDTO>(blogImageMap);
            return CreatedAtAction("GetById", new { id = blogImageReturn.ImageId }, blogImageReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogImage(int id, [FromBody] BlogImageRequestDTO blogImagedto)
        {
            if (blogImagedto == null)
            {
                return BadRequest();
            }

            // Lấy thực thể category hiện tại từ cơ sở dữ liệu
            var existingBlogImage = await _unitOfWork.BlogImageRepository.GetByIdAsync(id);
            if (existingBlogImage == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy category
            }

            // Cập nhật các thuộc tính của existingCategory bằng cách ánh xạ từ categoryDto
            _mapper.Map(blogImagedto, existingBlogImage);

            // Cập nhật vào cơ sở dữ liệu
            var updateResult = await _unitOfWork.BlogImageRepository.UpdateAsync(existingBlogImage);

            if (updateResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while updating blogImage");
                return StatusCode(500, ModelState); // Trả về 500 nếu có lỗi khi cập nhật
            }


            return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogImage(int id)
        {
            var blogImage = await _unitOfWork.BlogImageRepository.GetByIdAsync(id);

            if (blogImage == null)
            {
                return NotFound();
            }

            await _unitOfWork.BlogImageRepository.RemoveAsync(blogImage);

            return NoContent();
        }
        private bool BlogImageExists(int id)
        {
            return _unitOfWork.BlogImageRepository.GetByIdAsync(id) == null;
        }
    }
}
