using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _06_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly NAFContext _context;

        public CategoriaController(NAFContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllCategorias")]
        public async Task<ActionResult> GetAllCategorias()
        {
            var items = await _context.CATEGORIA.ToArrayAsync();

            if (!items.Any())
                return NotFound();

            var categorias = (from item in items
                              select new CategoriaDTO
                              {
                                  Code = item.CODIGO,
                                  Description = item.DESCRIPCION

                              }).ToList();

            return Ok(categorias);
        }

    }
}
