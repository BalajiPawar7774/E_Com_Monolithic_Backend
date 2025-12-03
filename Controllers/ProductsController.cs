using AutoMapper;
using E_Com_Monolithic.Dtos;
using E_Com_Monolithic.Helper;
using E_Com_Monolithic.Models;
using E_Com_Monolithic.Repositories;
using E_Com_Monolithic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Com_Monolithic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICommonRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ProductService _productService;
        public ProductsController(ICommonRepository<Product> productRepository, IMapper mapper, ProductService productService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Errors = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList()
                });
            }
            var product = _mapper.Map<Product>(dto);
            var addedProduct = await _productRepository.AddAsync(product);
            if (addedProduct != null)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.ProductId }, addedProduct);
            }
            return BadRequest("Failed to add product");
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with the id {id} not found");
            }
            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found");
            }
            return Ok(products);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Errors = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList()
                });
            }
            var product = _mapper.Map<Product>(dto);
            var updatedProduct = await _productRepository.UpdateAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound($"Product with the id {id} not found");
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _productRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound($"Product with the id {id} not found");
            }
            return NoContent();
        }

        [HttpGet("searchbyname")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProductsByName([FromQuery] string name)
        {
            var products = await _productService.SearchProductBYName(name);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found with the name containing '{name}'");
            }
            return Ok(products);
        }

        [HttpGet("searchProductByCategoryName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProductsByCategoryName([FromQuery] string categoryName, int page = 1, int pageSize = 10)
        {
            var products = await _productService.SearchProductBYCatogoryName(categoryName);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found with the category containing '{categoryName}'");
            }
            var totalCount = products.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedProducts = products
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            var vm = new ProductListPaginationModel
            {
                Products = pagedProducts,
                PageIndex = page,
                TotalPages = totalPages
            };


            return Ok(vm);
        }

        [HttpGet("searchProductByCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProductsByCategoryId([FromQuery] int categoryId)
        {
            var products = await _productService.SearchProductBYCategoryId(categoryId);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found with the category ID '{categoryId}'");
            }
            return Ok(products);
        }

        [HttpGet("searchUniversal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProductsUniversal( string  searchTerm, int page = 1, int pageSize = 10)
        {
            var dto = new ProductSearchRequestDto
            {
                SearchTerm = searchTerm
            };
            var products = await _productService.SearchProductsUniversal(dto);
            if (products == null || !products.Any())
            {
                return NotFound("No products found matching the search criteria");
            }
            var totalCount = products.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedProducts = products
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            var vm = new ProductListPaginationModel
            {
                Products = pagedProducts,
                PageIndex = page,
                TotalPages = totalPages
            };
            return Ok(vm);
        }
    }
}