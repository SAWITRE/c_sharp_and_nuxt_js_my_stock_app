using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStockApi.Data;
using MyStockApi.Models;
using MyStockApi.Dtos;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /*
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            // 1. ดึงข้อมูลสินค้าพร้อมหมวดหมู่ (Eager Loading)
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .ToListAsync();

            // 2. แปลงเป็น DTO
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.Category?.Id ?? null,
                CategoryName = p.Category?.Name ?? "ไม่มีหมวดหมู่"
            }).ToList();

            return Ok(productDtos);
        }*/
    [HttpGet]
    public async Task<IActionResult> GetProducts(string? search)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                     EF.Functions.ILike(x.Name ?? string.Empty, $"%{search}%"));
        }

        return Ok(await query.ToListAsync());
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        return Ok(MapToDto(product));
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createDto)
    {
        var product = MapToModel(createDto);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, MapToDto(product));
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDto updateDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        // อัปเดตข้อมูลจาก DTO
        product.Name = updateDto.Name;
        product.Price = updateDto.Price;
        product.StockQuantity = updateDto.StockQuantity;
        product.CategoryId = updateDto.CategoryId;

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

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // --- Mapping Helper Methods ---

    private Product MapToModel(CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId
        };
    }

    private ProductDto MapToDto(Product model)
    {
        return new ProductDto
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            StockQuantity = model.StockQuantity,
            CategoryId = model.Category?.Id ?? null, // ถ้าไม่มีหมวดหมู่ให้เป็น null
            CategoryName = model.Category?.Name ?? "ไม่มีหมวดหมู่"
        };
    }
}