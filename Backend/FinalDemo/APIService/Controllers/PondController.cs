﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository;
using AutoMapper;
using KCSAH.APIServer.Services;
using Domain.Models.Entity;
using Domain.Models.Dto.Response;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Update;

namespace KCSAH.APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PondController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PondService _getService;
        public PondController(UnitOfWork unitOfWork, IMapper mapper, PondService getService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _getService = getService;
        }

        [HttpGet("async/{id}")]
        public async Task<ActionResult<PondDTO>> GetByIdAsync(int id)
        {
            var pond = await _unitOfWork.PondRepository.GetByIdAsync(id);
            if (pond == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<PondDTO>(pond);
            return result;
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<PondDTO> GetById(int id)
        {
            var pond =  _unitOfWork.PondRepository.GetById(id);
            if (pond == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<PondDTO>(pond);
            return result;
        }

        [HttpGet("GetFishNumberByPondId/{id}")]
        public async Task<IActionResult> GetFood(int id)
        {
            var result = await _getService.GetNumberofFish(id);
            return Ok(result);
        }

        [HttpGet("ListKoiInPond/{id}")]
        public async Task<IActionResult> GetFishInPond(int id)
        {
            var result = await _getService.GetKoiInPond(id);
            if (result == null)
            {
                return NotFound();
            }
            var show = _mapper.Map<List<KoiDTO>>(result);
            return Ok(show);
        }

        [HttpGet("GetPondsByUserId/{id}")]
        public async Task<IActionResult> GetPondByUserIdAsync(string id)
        {
            var result = await _getService.GetPondByUserIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var show = _mapper.Map<List<PondDTO>>(result);
            return Ok(show);
        }

        [HttpGet("ListWaterParameter/{id}")]
        public async Task<IActionResult> WaterParameterListByPondId(int id)
        {
            var result = await _getService.GetPondWaterParameter(id);
            if (result == null)
            {
                return NotFound();
            }
            var show = _mapper.Map<List<WaterParameterDTO>>(result);
            return Ok(show);
        }
        [HttpPost]
        public async Task<ActionResult<Pond>> CreatePond([FromBody] PondRequestDTO ponddto)
        {
            if (ponddto == null)
            {
                return BadRequest(ModelState);
            }

            var pond = _unitOfWork.PondRepository.GetAll().Where(c => c.Name.ToUpper() == ponddto.Name.ToUpper()).FirstOrDefault();

            if (pond != null)
            {
                ModelState.AddModelError("", "This name has already existed.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pondMap = _mapper.Map<Pond>(ponddto);
            var createResult = await _unitOfWork.PondRepository.CreateAsync(pondMap);
            if (createResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            var pondShow = _mapper.Map<PondDTO>(pondMap);
            return CreatedAtAction("GetById",new {id = pondShow.PondId}, pondShow);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePond(int id, [FromBody] PondUpdateDTO ponddto)
        {
            if (ponddto == null)
            {
                return BadRequest();
            }

            var existingPond = await _unitOfWork.PondRepository.GetByIdAsync(id);
            if (existingPond == null)
            {
                return NotFound();
            }

            _mapper.Map(ponddto, existingPond);

            // Cập nhật vào cơ sở dữ liệu
            var updateResult = await _unitOfWork.PondRepository.UpdateAsync(existingPond);

            if (updateResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while updating Pond");
                return StatusCode(500, ModelState); // Trả về 500 nếu có lỗi khi cập nhật
            }

            return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePond(int id)
        {
            var pond = await _unitOfWork.PondRepository.GetByIdAsync(id);
            if (pond == null)
            {
                return NotFound();
            }
            await _unitOfWork.PondRepository.RemoveAsync(pond);

            return NoContent();
        }
    }
}
