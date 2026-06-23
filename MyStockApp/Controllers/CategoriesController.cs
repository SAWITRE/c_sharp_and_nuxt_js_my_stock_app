using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStockApi.Data;
using MyStockApi.Models;
using MyStockApi.Dtos;

namespace MyStockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        /*
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories.OrderBy(c => c.Id).ToListAsync();
            return Ok(categories.Select(c => MapToDto(c)));
        }*/

        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(string? search)
        {
           var query = _context.Categories
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                     EF.Functions.ILike(x.Name ?? string.Empty, $"%{search}%") ||
                     EF.Functions.ILike(x.Description ?? string.Empty, $"%{search}%"));
        }

        return Ok(await query.ToListAsync());
        }


        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            
            return Ok(MapToDto(category));
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createDto)
        {
            var category = MapToModel(createDto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, MapToDto(category));
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategoryDto updateDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // อัปเดตข้อมูลจาก DTO
            category.Name = updateDto.Name;
            category.Description = updateDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("เกิดข้อผิดพลาดในการอัปเดตข้อมูล");
            }

            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // --- Mapping Helper Methods ---

        private Category MapToModel(CreateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }

        private CategoryDto MapToDto(Category model)
        {
            return new CategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }
    }
}