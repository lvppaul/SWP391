﻿using AutoMapper;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Response;
using Domain.Models.Entity;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartDTO>>> GetAllAsync()
        {
            var cart = await _unitOfWork.CartRepository.GetAllAsync();
            var result = _mapper.Map<List<CartDTO>>(cart);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<CartDTO> ReturnCartById(int id)
        {
            var cart = _unitOfWork.CartRepository.GetByIdAsync(id);
            if (cart == null)
            {
                return NoContent();
            }
            var result = _mapper.Map<CartDTO>(cart);
            return Ok(result);
        }

        [HttpGet("CartId/{id}")]
        public async Task<ActionResult<CartDTO>> GetCartByIdAsync(int id)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<CartDTO>(cart);
            return Ok(result);
        }

        [HttpGet("UserId/{UserId}")]
        public async Task<ActionResult<CartDTO>> GetCartByUserIdAsync(string UserId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(UserId);
            if (cart == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<CartDTO>(cart);
            return Ok(result);
        }

        //[HttpGet("ShopId/{id}")]
        //public async Task<ActionResult<List<OrderDTO>>> GetOrderByShopIdAsync(int id)
        //{
        //    var order = await _unitOfWork.ShopRepository.GetOrderById(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    var result = _mapper.Map<List<OrderDTO>>(order);
        //    return Ok(result);
        //}

        //[HttpGet("UserId/{id}")]
        //public async Task<IActionResult> GetCartByUserIdAsync(string id)
        //{
        //    var result = await _getService.GetOrderByUserIdAsync(id);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    var show = _mapper.Map<List<OrderDTO>>(result);
        //    return Ok(show);
        //}
        [HttpPost]
        public async Task<ActionResult<CartDTO>> CreateOrder([FromBody] CartRequestDTO cartdto)
        {
            if (cartdto == null)
            {
                return BadRequest("Cart data cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCart = await _unitOfWork.CartRepository.GetByIdAsync(cartdto.UserId);

            if (existingCart != null)
            {
                return BadRequest("Cart for this user already exists.");
            }

            var cartMap = _mapper.Map<Cart>(cartdto);
            if (cartMap == null)
            {
                return BadRequest("Mapping to cart entity failed.");
            }
            decimal total = 0; // Khởi tạo giá trị cho total
            foreach (var detail in cartMap.CartItems)
            {
                var cartitem = await GetProductAsync(detail.ProductId);
                detail.ProductName = cartitem.Name;
                detail.Price = (decimal) cartitem.Price;
                detail.TotalPrice = (decimal)(detail.Price * detail.Quantity);
                detail.Thumbnail = cartitem.Thumbnail;
                total += (decimal)(detail.Price * detail.Quantity);
            }
            cartMap.TotalAmount = total;
            // Lưu vào cơ sở dữ liệu
            var createResult = await _unitOfWork.CartRepository.CreateAsync(cartMap);
            if (createResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            var cart = _mapper.Map<CartDTO>(cartMap);
            return CreatedAtAction(nameof(ReturnCartById), new { id = cart.CartId }, cart);
        }

        private async Task<Product> GetProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            return product;
        }
    }
}
