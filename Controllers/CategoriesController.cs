using AutoMapper;
using E_Com_Monolithic.Dtos;
using E_Com_Monolithic.Models;
using E_Com_Monolithic.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Com_Monolithic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICommonRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public CategoriesController(ICommonRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with the id {id} not found");
            }
            return Ok(category);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(dto);

            var addedCategory = await _categoryRepository.AddAsync(category);

            if (addedCategory != null)
            {
                return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.CategoryId }, addedCategory);
            }
            return BadRequest("Failed to add category");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with the id {id} not found");
            }
            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
