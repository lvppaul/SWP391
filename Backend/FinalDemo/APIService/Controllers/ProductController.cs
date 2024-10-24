﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository;
using AutoMapper;
using Domain.Models.Entity;
using Domain.Models.Dto.Response;
using Domain.Models.Dto.Request;
using Firebase.Auth;
using Firebase.Storage;
using Domain.Models.Dto.Update;

namespace KCSAH.APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllSync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var productDTOs = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDTOs);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductDTO>> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<ProductDTO>(product);
            var category = await GetCategoryAsync(product.CategoryId);
            result.category = category;
            return result;
        }
        [HttpGet("GetProductImageByProductId/{ProductId:int}")]
        public async Task<ActionResult<List<ProductImageDTO>>> GetProductImage(int ProductId)
        {
            var image = await _unitOfWork.ProductImageRepository.GetImageByProductId(ProductId);
            if(image == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<List<ProductImageDTO>>(image);
            return result;
        }

        [HttpGet("GetProductByCategoryId/{CategoryId}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductByCategoryId(int CategoryId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByCategoryId(CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<List<ProductDTO>>(product);
            return result;
        }

        [HttpGet("GetProductByCategoryIdInShop/{ShopId}/{CategoryId}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductByCategoryIdInShop(int CategoryId, int ShopId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByCategoryIdInShop(CategoryId, ShopId);
            if (product == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<List<ProductDTO>>(product);
            return result;
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<ProductDTO> GetById(int id)
        {
            var product =  _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<ProductDTO>(product);
            return result;
        }

        [HttpGet("ShopId/{id}")]
        public async Task<IActionResult> GetProductByShopIdAsync(int id)
        {
            var result = await _unitOfWork.ProductRepository.GetProductsBySID(id);
            if (result == null)
            {
                return NotFound();
            }
            var show = _mapper.Map<List<ProductDTO>>(result);
            return Ok(show);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductRequestDTO productdto)
        {
            if (productdto == null)
            {
                return BadRequest(ModelState);
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productdto.CategoryId);

            if (product == null)
            {
                return BadRequest("This product does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ánh xạ từ ProductDTO sang Product và liên kết với danh mục đã có hoặc mới tạo
            var productMap = _mapper.Map<Product>(productdto);
            productMap.CategoryId = product.CategoryId;  // Liên kết với danh mục hiện tại

            var createResult = await _unitOfWork.ProductRepository.CreateAsync(productMap);

            if (createResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving the product.");
                return StatusCode(500, ModelState);
            }
            var productShow = _mapper.Map<ProductDTO>(productMap);
            return CreatedAtAction("GetById",new { id = productShow.ProductId }, productShow);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO productdto)
        {
            if (productdto == null)
            {
                return BadRequest();
            }

            // Lấy thực thể product hiện tại từ cơ sở dữ liệu
            var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy sản phẩm
            }

            // Cập nhật các thuộc tính của existingProduct bằng cách ánh xạ từ productDto
            _mapper.Map(productdto, existingProduct);

            // Cập nhật vào cơ sở dữ liệu
            var updateResult = await _unitOfWork.ProductRepository.UpdateAsync(existingProduct);

            if (updateResult <= 0)
            {
                ModelState.AddModelError("", "Something went wrong while updating product");
                return StatusCode(500, ModelState); // Trả về 500 nếu có lỗi khi cập nhật
            }

            return NoContent(); // Trả về 200 OK với sản phẩm đã cập nhật
        }

        private async Task<CategoryDTO> GetCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            // Sử dụng AutoMapper để ánh xạ từ Category sang CategoryDTO
            return _mapper.Map<CategoryDTO>(category);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _unitOfWork.ProductRepository.RemoveAsync(product);

            return NoContent();
        }
    }
}

