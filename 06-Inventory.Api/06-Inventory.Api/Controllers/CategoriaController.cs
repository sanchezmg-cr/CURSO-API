using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using _06_Inventory.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

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

        [HttpGet("GetCategoria/{categoria}")]
        public async Task<ActionResult> GetCategoria(int categoria)
        {
            var item = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoria);

            if (item == null)
                return NotFound();

            CategoriaDTO record = new CategoriaDTO();

            record = new CategoriaDTO
            {
                Code = item.CODIGO,
                Description = item.DESCRIPCION
            };

            return Ok(record);
        }

        [HttpPut("SaveCategoria")]
        public async Task<ActionResult<CATEGORIA>> SaveCategoria(CategoriaDTO categoriaDTO)
        {
            var saveRecord = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoriaDTO.Code);

            if (saveRecord == null)
                return NotFound();

            saveRecord.DESCRIPCION = categoriaDTO.Description;
            saveRecord.ULT_MODIF_TSTAMP = DateTime.Now;
            saveRecord.ULT_MODIF_USUARIO = categoriaDTO.updateUser;

            try
            {
                _context.CATEGORIA.Update(saveRecord);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(saveRecord);
        }

        [HttpPost("createCategoria")]
        public async Task<ActionResult<CATEGORIA>> createCategoria(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
                return NoContent();

            CATEGORIA createCategoria;

            createCategoria = new CATEGORIA
            {
                CODIGO = categoriaDTO.Code,
                DESCRIPCION = categoriaDTO.Description,
                ULT_MODIF_TSTAMP = DateTime.Now,
                ULT_MODIF_USUARIO = categoriaDTO.updateUser
            };

            try
            {
                await _context.CATEGORIA.AddRangeAsync(createCategoria);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(createCategoria);
        }

        [HttpPost("deleteCategoria/{categoriaID}")]
        public async Task<ActionResult> deleteCategoria(int categoriaID)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();


            string sql = "BEGIN CAPBorrarCategoria(:pCategoria, :pResult); END;";

            OracleParameter pCategoria = new OracleParameter("pCategoria", categoriaID);
            OracleParameter pResult = new OracleParameter("pResult", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput) { Size = 4000 };

            await _context.Database.ExecuteSqlCommandAsync(sql, pCategoria, pResult);

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

        [HttpPost("ImportCategorias")]
        public async Task<ActionResult> ImportCategorias([FromBody] IEnumerable<CategoriaDTO> categoriaDTOs)
        {


            if (categoriaDTOs is null)
                return NotFound();

            else
            {
                var addCategorias = from A in categoriaDTOs
                                       select new CATEGORIA
                                         {
                                             CODIGO = A.Code,
                                             DESCRIPCION = A.Description,
                                             CREACION_TSTAMP = DateTime.Now,
                                             CREACION_USUARIO = A.CreateUser
                                         };
                try
                {
                    await _context.CATEGORIA.AddRangeAsync(addCategorias.ToList());
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
