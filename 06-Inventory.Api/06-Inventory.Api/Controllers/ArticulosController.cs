using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using _06_Inventory.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace _06_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly NAFContext _context;

        public ArticulosController(NAFContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllArticulos")]
        public async Task<ActionResult> GetAllArticulos()
        {
            var items = await _context.ARTICULOS.ToArrayAsync();

            if (!items.Any())
                return NotFound();

            var articulos = (from item in items
                              select new ArticulosDTO
                              {
                                  Code = item.CODIGO,
                                  Description = item.DESCRIPCION,
                                  Category = item.CATEGORIA,
                                  Brand = item.MARCA,
                                  Weight = item.PESO,
                                  BarCode = item.CODIGO_BARRAS
                              }).ToList();

            return Ok(articulos);
        }

        [HttpGet("GetArticulo/{articuloId}")]
        public async Task<ActionResult> GetCategoria(int articuloId)
        {
            var item = await _context.ARTICULOS.FirstOrDefaultAsync(x => x.CODIGO == articuloId);

            if (item == null)
                return NotFound();

            ArticulosDTO record = new ArticulosDTO();

            record = new ArticulosDTO
            {
                Code = item.CODIGO,
                Description = item.DESCRIPCION,
                Category = item.CATEGORIA,
                Brand = item.MARCA,
                Weight = item.PESO,
                BarCode = item.CODIGO_BARRAS
            };

            return Ok(record);
        }

        [HttpPut("SaveArticulo")]
        public async Task<ActionResult<ARTICULOS>> SaveArticulo(ArticulosDTO articulosDTO)
        {
            var saveRecord = await _context.ARTICULOS.FirstOrDefaultAsync(x => x.CODIGO == articulosDTO.Code);

            if (saveRecord == null)
                return NotFound();

            saveRecord.DESCRIPCION = articulosDTO.Description;
            saveRecord.ULT_MODIF_TSTAMP = DateTime.Now;
            saveRecord.ULT_MODIF_USUARIO = articulosDTO.updateUser;

            try
            {
                _context.ARTICULOS.Update(saveRecord);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(saveRecord);
        }

        [HttpPost("createArticulo")]
        public async Task<ActionResult<CATEGORIA>> createArticulo(ArticulosDTO articulosDTO)
        {
            if (articulosDTO == null)
                return NoContent();

            ARTICULOS createArticulo;

            createArticulo = new ARTICULOS
            {
                CODIGO = articulosDTO.Code,
                DESCRIPCION = articulosDTO.Description,
                CATEGORIA = articulosDTO.Category,
                MARCA = articulosDTO.Brand,
                PESO = articulosDTO.Weight,
                CODIGO_BARRAS = articulosDTO.BarCode,
                ULT_MODIF_TSTAMP = DateTime.Now,
                ULT_MODIF_USUARIO = articulosDTO.updateUser
            };

            try
            {
                await _context.ARTICULOS.AddRangeAsync(createArticulo);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(createArticulo);
        }

        [HttpPost("deleteArticulo/{ArticuloID}")]
        public async Task<ActionResult> deleteArticulo(int ArticuloID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();


            string sql = "BEGIN CAPBorrarArticulos(:pArticulo, :pResult); END;";

            OracleParameter pArticulo = new OracleParameter("pArticulo", ArticuloID);
            OracleParameter pResult = new OracleParameter("pResult", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput) { Size = 4000 };

            await _context.Database.ExecuteSqlCommandAsync(sql, pArticulo, pResult);

            if (pResult.Value.ToString().Equals("null"))
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
            };

            return Ok(pResult);
        }

        [HttpPost("ImportArticulos")]
        public async Task<ActionResult> ImportArticulos([FromBody] IEnumerable<ArticulosDTO> articulosDTOs)
        {


            if (articulosDTOs is null)
                return NotFound();

            else
            {
                var addArticulos = from A in articulosDTOs
                                    select new ARTICULOS
                                    {
                                        CODIGO = A.Code,
                                        DESCRIPCION = A.Description,
                                        CATEGORIA = A.Category,
                                        MARCA = A.Brand,
                                        PESO = A.Weight,
                                        CODIGO_BARRAS = A.BarCode,
                                        CREACION_TSTAMP = DateTime.Now,
                                        CREACION_USUARIO = A.CreateUser
                                    };
                try
                {
                    await _context.ARTICULOS.AddRangeAsync(addArticulos.ToList());
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();
            }
        }
    }
}
