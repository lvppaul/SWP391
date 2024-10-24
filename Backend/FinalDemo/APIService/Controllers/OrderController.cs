﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.KCSAH.Repository;
using KCSAH.APIServer.Dto;
using Domain.Models.Entity;
using Domain.Models.Dto.Response;
using Domain.Models.Dto.Request;
using Domain.Services;

namespace KCSAH.APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private UserService _getService;

        public OrderController(UnitOfWork unitOfWork, IMapper mapper, UserService getService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _getService = getService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllAsync()
        {
            var order = await _unitOfWork.OrderRepository.GetAllAsync();
            var result = _mapper.Map<List<OrderDTO>>(order);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<OrderDTO> ReturnOrderById(int id)
        {
            var order = _unitOfWork.OrderRepository.GetById(id);
            if (order == null)
            {
                return NoContent();
            }
            var result = _mapper.Map<OrderDTO>(order);
            return Ok(result);
        }

        [HttpGet("OrderId/{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByOrderIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<OrderDTO>(order);
            return Ok(result);
        }

        [HttpGet("ShopId/{id}")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrderByShopIdAsync(int id)
        {
            var order = await _unitOfWork.ShopRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<List<OrderDTO>>(order);
            return Ok(result);
        }

        [HttpGet("UserId/{id}")]
        public async Task<IActionResult> GetOrderByUserIdAsync(string id)
        {
            var result = await _getService.GetOrderByUserIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var show = _mapper.Map<List<OrderDTO>>(result);
            return Ok(show);
        }
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderRequestDTO orderdto)
        {
            if (orderdto == null)
            {
                return BadRequest("Order data cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderMap = _mapper.Map<Order>(orderdto);
            if (orderMap == null)
            {
                return BadRequest("Mapping to order entity failed.");
            }
            double total = 0; // Khởi tạo giá trị cho total
            foreach (var detail in orderMap.OrderDetails)
            {
                var product = await GetProductAsync(detail.ProductId);
                detail.UnitPrice = product.Price;
                total += detail.UnitPrice * detail.Quantity;
            }
            orderMap.TotalPrice = total;
            // Lưu vào cơ sở dữ liệu
            var createResult = await _unitOfWork.OrderRepository.CreateAsync(orderMap);
            if (createResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            var revenue = _mapper.Map<Revenue>(orderMap);
            revenue.CommissionFee = (total * 10/100);
            var createResultRevenue = await _unitOfWork.RevenueRepository.CreateAsync(revenue);
            if (createResultRevenue <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            var order = _mapper.Map<OrderDTO>(orderMap);
            return CreatedAtAction(nameof(ReturnOrderById), new { id = order.OrderId }, order);
        }

        private async Task<Product> GetProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            return product;
        }
    }
}
